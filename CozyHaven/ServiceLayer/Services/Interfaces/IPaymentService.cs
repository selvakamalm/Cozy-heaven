using DAL.Models.Main;

namespace ServiceLayer.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsByBookingIdAsync(int bookingId);
        Task<Payment> AddPaymentAsync(Payment payment);
        Task<bool> UpdatePaymentAsync(int id, Payment payment);
        Task<bool> DeletePaymentAsync(int id);
    }
}
