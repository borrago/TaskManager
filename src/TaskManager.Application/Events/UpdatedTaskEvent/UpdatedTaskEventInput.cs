﻿using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.UpdatedTaskEvent;

public class UpdatedTaskEventInput(Guid id, string title, string description, DateTime endDate, string status) : IEventInput
{
    public Guid Id { get; private set; } = id;
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description;
    public DateTime EndDate { get; private set; } = endDate;
    public string Status { get; private set; } = status;
}
