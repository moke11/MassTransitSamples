using System;
using MassTransit;

namespace SimpleStateMachine.Business
{
    public class EngineState : SagaStateMachineInstance
    {
        public State CurrentState { get; set; }
        public Guid CorrelationId { get; set; }
    }
}