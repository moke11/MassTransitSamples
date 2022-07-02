using System.Threading.Tasks;
using MassTransit;

namespace SimpleStateMachine.Car
{
    public class TireConsumer : IConsumer<RollTire>
    {
        public async Task Consume(ConsumeContext<RollTire> context)
        {
            await context.Publish(new TireRolling()
            {
                CorrelationId = context.Message.CorrelationId,
                Position = context.Message.Position
            });
        }
    }
}