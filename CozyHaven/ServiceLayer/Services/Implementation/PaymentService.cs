using DAL.DataAccess.Interface;
using DAL.Models.Main;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllAsync();
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBookingIdAsync(int bookingId)
        {
            return await _paymentRepository.GetPaymentsByBookingIdAsync(bookingId);
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> UpdatePaymentAsync(int id, Payment updatedPayment)
        {
            var existing = await _paymentRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.BookingId = updatedPayment.BookingId;
            existing.Amount = updatedPayment.Amount;
            existing.PaymentDate = updatedPayment.PaymentDate;
            existing.PaymentMethod = updatedPayment.PaymentMethod;

            _paymentRepository.Update(existing);
            await _paymentRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return false;

            _paymentRepository.Delete(payment);
            await _paymentRepository.SaveChangesAsync();
            return true;
        }
    }
}
