//
// Copyright (c) Microsoft.  All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using Hyak.Common;

namespace Microsoft.Azure.Test
{
    /// <summary>
    /// Chain of responsibility for finding a map to an undo function
    /// </summary>
    public abstract class OperationUndoHandler
    {
        OperationUndoHandler Inner
        {
            get;
            set;
        }

        /// <summary>
        /// Create a chain of undo handlers
        /// </summary>
        /// <param name="parameters">The set of undo handlers to add to the chain</param>
        /// <returns>The head of the chain of undo handlers</returns>
        public static OperationUndoHandler Create(params OperationUndoHandler[] parameters)
        {
            OperationUndoHandler first = new DelegatingOperationUndoHandler();
            OperationUndoHandler current = first;
            foreach (OperationUndoHandler next in parameters)
            {
                current.Add(next);
                current = next;
            }

            return first;
        }

        /// <summary>
        /// Trace an error in instantiating parameters for an undo function
        /// </summary>
        /// <param name="instance">The undo handler making the call</param>
        /// <param name="method">The method we are attempting to undo</param>
        /// <param name="parameters">The parameters passed to the method</param>
        protected static void TraceParameterError(object instance, string method, IDictionary<string, object> parameters)
        {
            TracingAdapter.Information("[{0}]: Unable to instantiate parameters to undo call to method {1}, using parameters: {2}", instance.GetType(), method, parameters.AsFormattedString());
        }

        /// <summary>
        /// Trace the call of a method that we cannot undo.  This may be expected behavior for some emthods.
        /// </summary>
        /// <param name="instance">The undo handler instance locating the undo</param>
        /// <param name="method">The method being undone</param>
        /// <param name="parameters">The parameters passed to the method</param>
        protected static void TraceUndoNotFound(object instance, string method, IDictionary<string, object> parameters)
        {
            TracingAdapter.Information("[{0}]: Unable to find undo call for method {1}, using parameters: {2}", instance.GetType(), method, parameters.AsFormattedString());
        }

        /// <summary>
        /// Trace an informational message that occurs during an undo operation
        /// </summary>
        /// <param name="instance">The undo handler instance</param>
        /// <param name="method">The method we are undoing</param>
        /// <param name="message">The message to send</param>
        protected static void TraceUndoMessage(object instance, string method, string message)
        {
            TracingAdapter.Information("[{0};Method{1}]: {2}", instance.GetType(), method, message);
        }

        /// <summary>
        /// Helper method to make assignment of parameters from parameter duictiohnaries more robust
        /// </summary>
        /// <typeparam name="T">The parameter type</typeparam>
        /// <param name="parameterName">The parameter name</param>
        /// <param name="parameters">The dictionary of parameter values</param>
        /// <returns>The value assigned from the parameter list, or null if none is specified</returns>
        protected static bool TryAssignParameter<T>(string parameterName, IDictionary<string, object> parameters, out T instanceToAssign)
        {
            instanceToAssign = default(T);
            if (parameters.ContainsKey(parameterName))
            {
                object candidate = parameters[parameterName];
                if (candidate.GetType().IsAssignableFrom(typeof(T)))
                {
                    instanceToAssign = (T)candidate;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add an undo handler to the current chain of undo handlers
        /// </summary>
        /// <param name="innerHandler">The handler to add</param>
        /// <returns>The head of the chain with the new handler added</returns>
        public virtual OperationUndoHandler Add(OperationUndoHandler innerHandler)
        {
            if (this.Inner == null)
            {
                this.Inner = innerHandler;
            }
            else
            {
                this.Inner.Add(innerHandler);
            }

            return this;
        }

        /// <summary>
        /// Lookup an undo function based on implementing instance, method name, and method poarameters
        /// </summary>
        /// <param name="client">The instance implementing the method</param>
        /// <param name="method">The name of the method begin called</param>
        /// <param name="parameters">The parameters passed to the method</param>
        /// <param name="undoAction">The undo action for the given method, if any can be found</param>
        /// <returns></returns>
        public virtual bool Lookup(object client, string method, IDictionary<string, object> parameters, out UndoableRestOperation undoOperation)
        {
            undoOperation = null;
            Action undoAction;
            bool result = TryFindUndoFunction(client, method, parameters, out undoAction);
            if (!result)
            {
                result = (null == this.Inner) ? false
                    : this.Inner.Lookup(client, method, parameters, out undoOperation);
            }
            else
            {
                undoOperation = this.CreateUndoableRestOperation(undoAction);
            }

            return result;
        }

        /// <summary>
        /// Create a robust rest client wrapper around an undo operation - the robust wrapper controls how and when operations are retried.
        /// </summary>
        /// <param name="undoAction">The undoAction to perform</param>
        /// <returns>A robust REST operation client wrapping the undo action</returns>
        public virtual UndoableRestOperation CreateUndoableRestOperation(Action undoAction)
        {
            return new UndoableRestOperation(undoAction);
        }

        /// <summary>
        /// Abstract function - in implementing classes, determines whether this handler applies to the 
        /// given client and if so, whether an undo method exists for it
        /// </summary>
        /// <param name="client">The instance implementing the method</param>
        /// <param name="method">The method name</param>
        /// <param name="parameters">The method parameters</param>
        /// <param name="undoFunction">The undo action for this operation, if one exists in the current handler</param>
        /// <returns>True if an undo function is found in the current handler, otherwise false</returns>
        protected abstract bool TryFindUndoFunction(object client, string method, IDictionary<string, object> parameters, out Action undoFunction);
    }

    /// <summary>
    /// OperationUndoHandler that uses reflection
    /// </summary>
    public abstract class ReflectingOperationUndoHandler : OperationUndoHandler
    {
        /// <summary>
        /// Determines whether the incoming operaton uses the specified client type and if so, whether we can find a matching undo function 
        /// for the operation
        /// </summary>
        /// <param name="client">The incoming client</param>
        /// <param name="method">The called method</param>
        /// <param name="parameters">The parameters in the call</param>
        /// <param name="undoFunction">The fuinction which rolls back the incoming call</param>
        /// <returns></returns>
        protected override bool TryFindUndoFunction(object client, string method, IDictionary<string, object> parameters, out Action undoFunction)
        {
            undoFunction = null;
            if (this.TryMatchClient(client))
            {
                return this.TryFindUndoAction(client, method, parameters, out undoFunction);
            }

            return false;
        }

        /// <summary>
        /// Name of the client type, used to construct the client for undo functions.
        /// </summary>
        protected virtual string ClientTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Namespace of the client type, used to construct the client for undo functions.
        /// </summary>
        protected virtual string ClientTypeNamespace
        {
            get;
            set;
        }

        /// <summary>
        /// See if the client used in the operation matches the expected client type
        /// </summary>
        /// <param name="client">The incoming client</param>
        /// <returns>True if the client type matches the expected client type handled by this handler, otherwise false.</returns>
        protected virtual bool TryMatchClient(object client)
        {
            Type candidateType = client.GetType();
            return string.Equals(candidateType.Name, this.ClientTypeName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(candidateType.Namespace, this.ClientTypeNamespace, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Given an operation trace, try to find an undo action for the given operation.
        /// </summary>
        /// <param name="client">The incoming client</param>
        /// <param name="method">The method called</param>
        /// <param name="parameters">The parameters passed to the method</param>
        /// <param name="undoAction">The resulting undo Action, if any.</param>
        /// <returns></returns>
        protected abstract bool TryFindUndoAction(object client, string method, IDictionary<string, object> parameters, out Action undoAction);

        /// <summary>
        /// Create a client for undo operationsfrom the incomign client used for the operation
        /// </summary>
        /// <param name="operationsClient"></param>
        /// <returns></returns>
        protected virtual object CreateServiceClient(object operationsClient)
        {
            PropertyInfo clientProperty = operationsClient.GetType().GetProperty("Client");
            object oldServiceClient = clientProperty.GetValue(operationsClient);
            Type clientType = clientProperty.PropertyType;
            PropertyInfo credentialProperty = clientType.GetProperty("Credentials");
            PropertyInfo baseUriProperty = clientType.GetProperty("BaseUri");
            ConstructorInfo constructor = clientType.GetConstructor(new Type[] { credentialProperty.PropertyType, baseUriProperty.PropertyType });
            return constructor.Invoke(new object[] { credentialProperty.GetValue(oldServiceClient), baseUriProperty.GetValue(oldServiceClient) });
        }

        /// <summary>
        /// Determine the underlyign client type from the client
        /// </summary>
        /// <param name="operationsClient"></param>
        /// <returns>the type of the underlyign client</returns>
        protected virtual Type GetServiceClientType(object operationsClient)
        {
            return operationsClient.GetType().GetProperty("Client").PropertyType;

        }
    }

    /// <summary>
    /// Abstract class for implenting simple undo handlers where operation and undo operation are implemented 
    /// in the same instance
    /// </summary>
    /// <typeparam name="T">The type of the implementing class for operation and undo operation</typeparam>
    public abstract class SimpleTypedOperationUndoHandler<T> : OperationUndoHandler where T : class
    {
        /// <summary>
        /// The function which returns undo operations for implementing class T
        /// </summary>
        /// <param name="client">The implementing class</param>
        /// <param name="method">The method name of the operation</param>
        /// <param name="parameters">The operation parameters</param>
        /// <param name="undoAction">The undo function for this operation, if any</param>
        /// <returns>True if an undo function is found, otherwise false</returns>
        protected abstract bool DoLookup(T client, string method, IDictionary<string, object> parameters, out Action undoAction);

        /// <summary>
        /// Finds an undo function implemented in the same class as the operation
        /// </summary>
        /// <param name="client">The implementing class for operation and undo operation</param>
        /// <param name="method">The name of the method</param>
        /// <param name="parameters">The method parameters</param>
        /// <param name="undoFunction">The undo action for the given method, if any exists</param>
        /// <returns>True if an undo function is found for the method, otherwise false</returns>
        protected override bool TryFindUndoFunction(object client, string method, IDictionary<string, object> parameters, out Action undoFunction)
        {
            undoFunction = null;
            T typedClient = client as T;
            if (typedClient != null)
            {
                return this.DoLookup(typedClient, method, parameters, out undoFunction);
            }

            return false;
        }
    }

    /// <summary>
    /// Abstract class for implementing undo operations on service clients - the client implementing the undo 
    /// action may be different that the client implementing the operation to undo
    /// </summary>
    /// <typeparam name="T">The class implementing the operation</typeparam>
    /// <typeparam name="U">A WAML service class which may implement undo methods</typeparam>
    public abstract class ComplexTypedOperationUndoHandler<T, U> : OperationUndoHandler
        where T : class
        where U : ServiceClient<U>
    {
        /// <summary>
        /// Finds an undo function implemented in the same class as the operation
        /// </summary>
        /// <param name="client">The implementing class for operation and undo operation</param>
        /// <param name="method">The name of the method</param>
        /// <param name="parameters">The method parameters</param>
        /// <param name="undoFunction">The undo action for the given method, if any exists</param>
        /// <returns>True if an undo function is found for the method, otherwise false</returns>
        protected override bool TryFindUndoFunction(object client, string method, IDictionary<string, object> parameters, out Action undoFunction)
        {
            undoFunction = null;
            T operations = client as T;
            IServiceOperations<U> parent = client as IServiceOperations<U>;
            if (operations != null && parent != null)
            {
                return this.DoLookup(parent, method, parameters, out undoFunction);
            }

            return false;
        }

        /// <summary>
        /// The function which returns undo operations for implementing class T
        /// </summary>
        /// <param name="client">The implementing class</param>
        /// <param name="method">The method name of the operation</param>
        /// <param name="parameters">The operation parameters</param>
        /// <param name="undoAction">The undo function for this operation, if any</param>
        /// <returns>True if an undo function is found, otherwise false</returns>
        protected abstract bool DoLookup(IServiceOperations<U> client, string method, IDictionary<string, object> parameters, out Action undoFunction);

        /// <summary>
        /// Get a usabele service client from the operations client returned in method traces.
        /// </summary>
        /// <param name="operations">The operations client used for the incoming request.</param>
        /// <returns></returns>
        protected abstract U GetClientFromOperations(IServiceOperations<U> operations);
    }
}
