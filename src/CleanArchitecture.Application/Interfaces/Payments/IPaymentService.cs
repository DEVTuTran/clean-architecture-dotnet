using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.Payments;

public interface IPaymentService
{
    Task<PaymentResult> ModifyBuyerAsync(object buyer, int organizationId);
    Task<PaymentResult> RegisterBuyerAsync(object buyer);
}

public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
}



