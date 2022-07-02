using System;
using System.Collections.Generic;
using MassTransit;

namespace SimpleStateMachine.Car
{
    public class CarState : SagaStateMachineInstance
    {
        public CarState()
        {
            Tires = new HashSet<Tire>()
            {
                new Tire(TirePosition.FrontLeft),
                new Tire(TirePosition.FrontRight),
                new Tire(TirePosition.RearLeft),
                new Tire(TirePosition.RearRight)
            };
        }

        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        public HashSet<Tire> Tires { get; set; }
    }

    public class Tire
    {
        public Tire(TirePosition position)
        {
            Position = position;
            Moving = false;
        }

        public TirePosition Position { get; set; }
        public bool Moving { get; set; }
    }

    public enum TirePosition
    {
        FrontLeft,
        FrontRight,
        RearLeft,
        RearRight
    }
}