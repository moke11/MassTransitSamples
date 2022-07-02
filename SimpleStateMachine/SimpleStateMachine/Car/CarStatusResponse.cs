using System;

namespace SimpleStateMachine.Car
{
    public class CarStatusResponse
    {
        public string State { get; set; }
        public Guid CorrelationId { get; set; }
    }
}