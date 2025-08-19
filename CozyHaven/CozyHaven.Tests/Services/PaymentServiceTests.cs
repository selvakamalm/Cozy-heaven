using DAL.DataAccess.Interface;
using DAL.Models.Main;
using Moq;
using NUnit.Framework.Legacy;
using ServiceLayer.Services.Implementation;

namespace CozyHaven.Tests.Services
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private Mock<IPaymentRepository> _paymentRepoMock = null!;
        private PaymentService _paymentService = null!;

        [SetUp]
        public void SetUp()
        {
            _paymentRepoMock = new Mock<IPaymentRepository>();
            _paymentService = new PaymentService(_paymentRepoMock.Object);
        }

        [Test]
        public async Task GetAllPaymentsAsync_ShouldReturnPayments()
        {
            var payments = new List<Payment> { new Payment { Id = 1 }, new Payment { Id = 2 } };
            _paymentRepoMock.Setup(p => p.GetAllAsync()).ReturnsAsync(payments);

            var result = await _paymentService.GetAllPaymentsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetPaymentByIdAsync_ShouldReturnPayment_WhenExists()
        {
            var payment = new Payment { Id = 1 };
            _paymentRepoMock.Setup(p => p.GetByIdAsync(1)).ReturnsAsync(payment);

            var result = await _paymentService.GetPaymentByIdAsync(1);

            Assert.That(result, Is.EqualTo(payment));
        }

        [Test]
        public async Task GetPaymentByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            _paymentRepoMock.Setup(p => p.GetByIdAsync(99)).ReturnsAsync((Payment?)null);

            var result = await _paymentService.GetPaymentByIdAsync(99);

            ClassicAssert.IsNull(result);
        }

        [Test]
        public async Task GetPaymentsByBookingIdAsync_ShouldReturnPayments()
        {
            var payments = new List<Payment> { new Payment { BookingId = 5 } };
            _paymentRepoMock.Setup(p => p.GetPaymentsByBookingIdAsync(5)).ReturnsAsync(payments);

            var result = await _paymentService.GetPaymentsByBookingIdAsync(5);

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task AddPaymentAsync_ShouldAddAndReturnPayment()
        {
            var payment = new Payment { Id = 1, Amount = 3000 };
            _paymentRepoMock.Setup(p => p.AddAsync(payment)).Returns(Task.CompletedTask);
            _paymentRepoMock.Setup(p => p.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _paymentService.AddPaymentAsync(payment);

            Assert.That(result, Is.EqualTo(payment));
            _paymentRepoMock.Verify(p => p.AddAsync(payment), Times.Once);
            _paymentRepoMock.Verify(p => p.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task UpdatePaymentAsync_ShouldReturnFalse_WhenNotFound()
        {
            _paymentRepoMock.Setup(p => p.GetByIdAsync(1)).ReturnsAsync((Payment?)null);

            var result = await _paymentService.UpdatePaymentAsync(1, new Payment());

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task UpdatePaymentAsync_ShouldReturnTrue_WhenUpdated()
        {
            var existing = new Payment { Id = 1 };
            var updated = new Payment
            {
                Id = 1,
                Amount = 2000,
                BookingId = 2,
                PaymentDate = DateTime.Now,
                PaymentMethod = "UPI"
            };

            _paymentRepoMock.Setup(p => p.GetByIdAsync(1)).ReturnsAsync(existing);
            _paymentRepoMock.Setup(p => p.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _paymentService.UpdatePaymentAsync(1, updated);

            ClassicAssert.IsTrue(result);
            _paymentRepoMock.Verify(p => p.Update(It.Is<Payment>(x =>
                x.Amount == 2000 &&
                x.BookingId == 2 &&
                x.PaymentMethod == "UPI"
            )), Times.Once);
        }

        [Test]
        public async Task DeletePaymentAsync_ShouldReturnFalse_WhenNotFound()
        {
            _paymentRepoMock.Setup(p => p.GetByIdAsync(10)).ReturnsAsync((Payment?)null);

            var result = await _paymentService.DeletePaymentAsync(10);

            ClassicAssert.IsFalse(result);
        }

        [Test]
        public async Task DeletePaymentAsync_ShouldReturnTrue_WhenDeleted()
        {
            var payment = new Payment { Id = 1 };
            _paymentRepoMock.Setup(p => p.GetByIdAsync(1)).ReturnsAsync(payment);
            _paymentRepoMock.Setup(p => p.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _paymentService.DeletePaymentAsync(1);

            ClassicAssert.IsTrue(result);
            _paymentRepoMock.Verify(p => p.Delete(payment), Times.Once);
            _paymentRepoMock.Verify(p => p.SaveChangesAsync(), Times.Once);
        }
    }
}
