using System;
using MassTransit;
using SimpleStateMachine.Messages;

namespace SimpleStateMachine.Business
{
    public class EngineStateMachine : MassTransitStateMachine<EngineState>
    {
        public EngineStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => Started, x => x.CorrelateById(m => m.Message.EngineId));
            Event(() => Stopped, x => x.CorrelateById(m => m.Message.EngineId));

            Initially(
                When(Started)
                    .Then(context => {Console.WriteLine($"started and running {context.Instance.CorrelationId}");})
                    .TransitionTo(Running));

            During(Running,
                When(Stopped)
                    .Then(context => {Console.WriteLine($"stopped {context.Instance.CorrelationId}");})
                    .Finalize());
        }

        // ReSharper disable UnassignedGetOnlyAutoProperty
        public State Running { get; }
        public Event<StartEngine> Started { get; }
        public Event<StopEngine> Stopped { get; }
    }
}