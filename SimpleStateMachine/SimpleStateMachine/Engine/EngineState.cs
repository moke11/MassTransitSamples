using System;
using MassTransit;

namespace SimpleStateMachine.Engine
{
    public class EngineState : SagaStateMachineInstance
    {
        public State CurrentState { get; set; }
        public Guid CorrelationId { get; set; }
    }
}