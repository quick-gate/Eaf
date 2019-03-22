using Castle.DynamicProxy;
using QGate.Eaf.Domain.Metadatas.Models;
using QGate.Eaf.Domain.Metadatas.Services;
using System;
using System.Collections.Generic;

namespace QGate.Eaf.Core.Metadatas.Services
{
    public class EntityDescriptorFactory : IEntityDescriptorFactory
    {
        private static readonly ProxyGenerator _proxy = new ProxyGenerator();
        private static IDictionary<Type, EntityDescriptor> _entityDescriptors = new Dictionary<Type, EntityDescriptor>();
        private static EntityDescriptorInterceptor _entityDescriptorInterceptor;
        public void Add<TEntityDescriptor>(TEntityDescriptor descriptor) where TEntityDescriptor : EntityDescriptor
        {
            var descriptorType = descriptor.GetType();
            if (!_entityDescriptors.ContainsKey(descriptorType))
            {
                _entityDescriptors.Add(descriptorType, descriptor);
            }
        }

        public TEntityDescriptor Get<TEntityDescriptor>() where TEntityDescriptor : EntityDescriptor
        {
            _entityDescriptors.TryGetValue(typeof(TEntityDescriptor), out EntityDescriptor descriptor);
            return (TEntityDescriptor) descriptor;
        }

        public TEntityDescriptor Get<TEntityDescriptor>(TEntityDescriptor descriptor, IEntityDescriptorContext context) where TEntityDescriptor : EntityDescriptor
        {
            if(_entityDescriptorInterceptor == null)
            {
                _entityDescriptorInterceptor = new EntityDescriptorInterceptor();
            }
            var options = new ProxyGenerationOptions();
            options.AddMixinInstance(context);
            return _proxy.CreateClassProxyWithTarget(descriptor, options, _entityDescriptorInterceptor);
        }

        public IEnumerable<EntityDescriptor> GetDescriptors()
        {
            return _entityDescriptors.Values;
        }
    }
}
