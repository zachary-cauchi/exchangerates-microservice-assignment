﻿namespace Exchange.API.Models
{
    public class Currency : IAggregateRoot
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Code { get; set; } = default!;
    }
}
