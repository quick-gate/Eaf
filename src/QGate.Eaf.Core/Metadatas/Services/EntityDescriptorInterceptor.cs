using Castle.DynamicProxy;
using QGate.Eaf.Domain.Exceptions;
using QGate.Eaf.Domain.Metadatas.Models;

namespace QGate.Eaf.Core.Metadatas.Services
{
    public class EntityDescriptorInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if(invocation.InvocationTarget is EntityDescriptor)
            {
                invocation.Proceed();
                return;
            }
            var context = invocation.InvocationTarget as IEntityDescriptorContext;
            if(context == null)
            {
                throw new EafException("Cannot read Context from Entity Descriptor Proxy");
            }

            if (invocation.Arguments.Length > 0)
            {
                var relationDescriptor = invocation.GetArgumentValue(0);
                context.RelationDescriptor = (RelationDescriptor) relationDescriptor;
            }
            invocation.ReturnValue = context.RelationDescriptor;

        }
    }
}
