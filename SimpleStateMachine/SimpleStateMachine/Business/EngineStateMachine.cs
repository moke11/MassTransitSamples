using System;
using MassTransit;

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
                    .Then(context => {Console.WriteLine($"started and running {context.Saga.CorrelationId}");})
                    .TransitionTo(Running));

            During(Running,
                When(Stopped)
                    .Then(context => {Console.WriteLine($"stopped {context.Saga.CorrelationId}");})
                    .Finalize());
        }

        // ReSharper disable UnassignedGetOnlyAutoProperty
        public State Running { get; }
        public Event<StartEngine> Started { get; }
        public Event<StopEngine> Stopped { get; }
    }
}