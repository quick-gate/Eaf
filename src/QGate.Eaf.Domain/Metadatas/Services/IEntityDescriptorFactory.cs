using QGate.Eaf.Domain.Metadatas.Models;
using System;
using System.Collections.Generic;

namespace QGate.Eaf.Domain.Metadatas.Services
{
    public interface IEntityDescriptorFactory
    {
        IEnumerable<EntityDescriptor> GetDescriptors();
        void Add<TEntityDescriptor>(TEntityDescriptor descriptor) where TEntityDescriptor : EntityDescriptor;
        TEntityDescriptor Get<TEntityDescriptor>() where TEntityDescriptor : EntityDescriptor;
        /// <summary>
        /// Get Entity descriptor proxy with context. For returned descriptor can be called (IEntityDescriptorContext) entityDescriptor
        /// </summary>
        /// <typeparam name="TEntityDescriptor"></typeparam>
        /// <param name="descriptor"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        TEntityDescriptor Get<TEntityDescriptor>(TEntityDescriptor descriptor, IEntityDescriptorContext context) where TEntityDescriptor : EntityDescriptor;
    }
}
