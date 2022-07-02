using System;

namespace SimpleStateMachine.Car
{
    public class RollTire
    {
        public TirePosition Position { get; set; }
        public Guid CorrelationId { get; set; }
    }
}