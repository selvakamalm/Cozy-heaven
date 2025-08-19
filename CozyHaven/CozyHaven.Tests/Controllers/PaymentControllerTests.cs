using DAL.Models.Main;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Legacy;
using ServiceLayer.Controllers;
using ServiceLayer.Services.Interfaces;

namespace CozyHaven.Tests.Controllers
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private Mock<IPaymentService> _paymentServiceMock = null!;
        private PaymentController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _controller = new PaymentController(_paymentServiceMock.Object);
        }

        [Test]
        public async Task GetAllPayments_ShouldReturnAllPayments()
        {
            var payments = new List<Payment> { new Payment { Id = 1 }, new Payment { Id = 2 } };
            _paymentServiceMock.Setup(x => x.GetAllPaymentsAsync()).ReturnsAsync(payments);

            var result = await _controller.GetAllPayments() as OkObjectResult;

            ClassicAssert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(((List<Payment>)result.Value!).Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetPaymentById_WhenFound_ShouldReturnPayment()
        {
            var payment = new Payment { Id = 1 };
            _paymentServiceMock.Setup(x => x.GetPaymentByIdAsync(1)).ReturnsAsync(payment);

            var result = await _controller.GetPaymentById(1) as OkObjectResult;

           ClassicAssert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo(payment));
        }

        [Test]
        public async Task GetPaymentById_WhenNotFound_ShouldReturnNotFound()
        {
            _paymentServiceMock.Setup(x => x.GetPaymentByIdAsync(999)).ReturnsAsync((Payment?)null);

            var result = await _controller.GetPaymentById(999);

            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetPaymentsByBookingId_ShouldReturnPayments()
        {
            var payments = new List<Payment> { new Payment { BookingId = 1 } };
            _paymentServiceMock.Setup(x => x.GetPaymentsByBookingIdAsync(1)).ReturnsAsync(payments);

            var result = await _controller.GetPaymentsByBookingId(1) as OkObjectResult;

            ClassicAssert.IsNotNull(result);
            Assert.That(((List<Payment>)result.Value!).Count, Is.EqualTo(1));
        }

        [Test]
        public async Task AddPayment_ShouldReturnCreatedResult()
        {
            var payment = new Payment { Id = 1, BookingId = 1 };
            _paymentServiceMock.Setup(x => x.AddPaymentAsync(payment)).ReturnsAsync(payment);

            var result = await _controller.AddPayment(payment) as CreatedAtActionResult;

            ClassicAssert.IsNotNull(result);
            Assert.That(result.ActionName, Is.EqualTo(nameof(_controller.GetPaymentById)));
            Assert.That(result.Value, Is.EqualTo(payment));
        }

        [Test]
        public async Task UpdatePayment_WhenSuccessful_ShouldReturnNoContent()
        {
            var payment = new Payment { Id = 1 };
            _paymentServiceMock.Setup(x => x.UpdatePaymentAsync(1, payment)).ReturnsAsync(true);

            var result = await _controller.UpdatePayment(1, payment);

            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }


        [Test]
        public async Task DeletePayment_WhenSuccessful_ShouldReturnNoContent()
        {
            _paymentServiceMock.Setup(x => x.DeletePaymentAsync(1)).ReturnsAsync(true);

            var result = await _controller.DeletePayment(1);

            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeletePayment_WhenNotFound_ShouldReturnNotFound()
        {
            _paymentServiceMock.Setup(x => x.DeletePaymentAsync(1)).ReturnsAsync(false);

            var result = await _controller.DeletePayment(1);

            ClassicAssert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
