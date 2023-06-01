using MassTransit;
using OrderStateMachine.Domain;
using OrderStateMachine.Domain.Events;

namespace OrderStateMachine.Host;

public class OrderStateMachineInstance : MassTransitStateMachine<OrderStateInstance>
{
    public OrderStateMachineInstance()
    {
        // orderrecieved -> productreserved -> paymentcompleted -> order completed.
        // orderrecievedFault <- ..fault <- ...fault <- ..fault 
        
        Event(() => OrderReceivedEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.OrderId));
        Event(() => OrderReceivedFaultEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.OrderId));

        Event(() => ProductReservedEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.OrderId));
        Event(() => ProductReservedFaultEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.OrderId));

        Event(() => PaymentCompletedEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.OrderId));
        Event(() => PaymentCompletedFaultEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.OrderId));

        Event(() => OrderCompletedEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.OrderId));

        Event(() => OrderCompletedFaultEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.OrderId));


        Event(() => SagaStateRequestedEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.OrderId));
        Event(() => SagaPaymentInfoUpdatedEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.OrderId));
        
        Event(() => SagaProductInfoUpdatedEvent,
            x => x.CorrelateById(context => context.InitiatorId ?? context.Message.OrderId));

        InstanceState(x => x.CurrentState);

        During(Initial,
            When(OrderReceivedEvent)
                .Then(x => x.Saga.OrderStartDate = DateTime.Now)
                .TransitionTo(OrderReceived));

        During(OrderReceived,
            When(ProductReservedEvent)
                .TransitionTo(ProductReserved));

        During(ProductReserved,
            When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompleted));

        During(PaymentCompleted,
            When(OrderCompletedEvent)
                .Then(x => x.Saga.OrderEndDate = DateTime.Now)
                .TransitionTo(OrderCompleted));


        DuringAny(When(SagaPaymentInfoUpdatedEvent)
            .Then(context => context.Saga.ReceiptId = context.Message.ReceiptId));
        

        DuringAny(When(OrderCompletedFaultEvent)
            .TransitionTo(OrderCompletedFaulted)
            .Then(context => context.Publish<Fault<PaymentCompleted>>(new { context.MessageId })));

        DuringAny(When(PaymentCompletedFaultEvent)
            .TransitionTo(PaymentCompletedFaulted)
            .Then(context => context.Publish<Fault<ProductReserved>>(new { context.MessageId })));

        DuringAny(When(ProductReservedFaultEvent)
            .TransitionTo(ProductAvailabilityCheckedFaulted)
            .Then(context => context.Publish<Fault<OrderReceived>>(new { context.MessageId })));

        DuringAny(When(OrderReceivedFaultEvent)
            .TransitionTo(OrderReceivedFaulted));
    }

    public State OrderReceived { get; set; }
    public State ProductReserved { get; set; }
    public State PaymentCompleted { get; set; }
    public State OrderCompleted { get; set; }

    public State OrderReceivedFaulted { get; set; }
    public State ProductAvailabilityCheckedFaulted { get; set; }
    public State PaymentCompletedFaulted { get; set; }
    public State OrderCompletedFaulted { get; set; }
    public Event<OrderReceived> OrderReceivedEvent { get; set; }
    public Event<ProductReserved> ProductReservedEvent { get; set; }
    public Event<PaymentCompleted> PaymentCompletedEvent { get; set; }
    public Event<OrderCompleted> OrderCompletedEvent { get; set; }
    public Event<SagaStateRequested> SagaStateRequestedEvent { get; set; }
    public Event<SagaPaymentInfoUpdated> SagaPaymentInfoUpdatedEvent { get; set; }
    public Event<SagaProductInfoUpdated> SagaProductInfoUpdatedEvent { get; set; }

    public Event<Fault<OrderReceived>> OrderReceivedFaultEvent { get; set; }
    public Event<Fault<ProductReserved>> ProductReservedFaultEvent { get; set; }
    public Event<Fault<PaymentCompleted>> PaymentCompletedFaultEvent { get; set; }
    public Event<Fault<OrderCompleted>> OrderCompletedFaultEvent { get; set; }
    public Event<Fault<SagaStateRequested>> SagaStateRequestedFaultEvent { get; set; }
}