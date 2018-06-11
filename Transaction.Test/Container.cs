﻿using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Transaction.Test
{
    public sealed class Container
    {
        #region Private Fields

        #endregion

        #region Constructor

        public IUnityContainer IocContainer { get; } = new UnityContainer();

        /// <summary>
        /// Prevents the creation of an instance of the Container type.
        /// </summary>
        private Container()
        {
        }

        /// <summary>
        /// Initializes static members of the Container type.
        /// </summary>
        static Container()
        {
            Instance.IocContainer.RegisterInstance(Instance, new ContainerControlledLifetimeManager());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton instance of the Container class.
        /// </summary>
        public static Container Instance { get; } = new Container();

        #endregion

        #region Public Methods

        /// <summary>
        /// Registers the TBase type to the TImplementation type.
        /// </summary>
        /// <typeparam name="TBase">The base type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        public void Register<TBase, TImplementation>() where TImplementation : TBase
        {
            this.IocContainer.RegisterType(typeof(TBase), typeof(TImplementation), new TransientLifetimeManager());
        }

        /// <summary>
        /// Registers the TBase type to a singleton of the provided value.
        /// </summary>
        /// <typeparam name="TBase">The base type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        /// <param name="value">The singleton value.</param>
        public void Register<TBase, TImplementation>(TImplementation value) where TImplementation : TBase
        {
            this.IocContainer.RegisterInstance(typeof(TBase), value, new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Resolves the provided type.
        /// </summary>
        /// <typeparam name="T">Resolves the type T.</typeparam>
        /// <returns>The resolved value.</returns>
        public T Resolve<T>()
        {
            return (T)this.IocContainer.Resolve(typeof(T));
        }

        /// <summary>
        /// Resolves the type T using the provided parameters.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public T Resolve<T>(Dictionary<string, object> parameters)
        {
            var parameterOverrides = new ParameterOverrides();
            foreach (var parameter in parameters)
            {
                parameterOverrides.Add(parameter.Key, parameter.Value);
            }

            return (T)this.IocContainer.Resolve(typeof(T), parameterOverrides.OnType<T>());
        }

        /// <summary>
        /// Injects in constructor TUsage the type TBase
        /// </summary>
        /// <typeparam name="TUsage"></typeparam>
        /// <typeparam name="TBase"></typeparam>
        public void RegisterInConstructor<TUsage, TBase>()
        {
            this.IocContainer.RegisterType<TUsage>(new InjectionConstructor(Instance.Resolve<TBase>()));
        }

        /// <summary>
        /// Register a class with an interface which are Open Generics
        /// </summary>
        /// <param name="basic">Open Generic Interface</param>
        /// <param name="implementation">Open Generic Implementation</param>
        public void Register(Type basic, Type implementation)
        {
            this.IocContainer.RegisterType(basic, implementation, new TransientLifetimeManager());
        }

        #endregion
    }
}