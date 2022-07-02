using System;

namespace SimpleStateMachine.Car
{
    public class TireRolling
    {
        public TirePosition Position { get; set; }
        public Guid CorrelationId { get; set; }
    }
}