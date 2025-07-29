﻿namespace DomainValidation.Tests.Models;
public class CreateOrder
{
    public const string CustomerIdIsRequired =
        "Customer ID is required";
    public const string CustomerIdFixedLength =
        "The length of the client Id must be 5.";
    public const string CustomerIdHasMaxLength =
        "The length of the client ID must be a maximum of 5 characters.";
    public const string CustomerIdHasMinLength =
        "The length of the client ID must be at least 5 characters.";
    public const string CustomerIdMatch =
        "The customer ID must have 5 digits.";
    public const string NotEmpty =
        "At least one item of the order detail is required.";



    public string CustomerId { get; set; }
    public string ShipAddress { get; set; }
    public string ShipCity { get; set; }
    public string ShipCountry { get; set; }
    public string ShipPostalCode { get; set; }
    public IEnumerable<CreateOrderDetail> OrderDetails { get; set; }
}

