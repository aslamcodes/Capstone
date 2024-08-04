using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Payments;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.Payments;

[TestFixture]
public class PaymentRepoTests
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EduQuestContext(options);
        _repo = new PaymentRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private EduQuestContext _context;
    private PaymentRepo _repo;

    [Test]
    public async Task Add_ShouldAddPaymentToDatabase()
    {
        // Arrange
        var payment = new Payment
        {
            OrderId = 1,
            PaymentTransactionId = "TRANS123",
            PaymentStatus = PaymentStatusEnum.Pending,
            ProcessedAt = null,
            Amount = 100.0f
        };

        // Act
        var result = await _repo.Add(payment);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.OrderId, Is.EqualTo(1));
        Assert.That(result.PaymentTransactionId, Is.EqualTo("TRANS123"));
        Assert.That(result.PaymentStatus, Is.EqualTo(PaymentStatusEnum.Pending));
        Assert.That(result.ProcessedAt, Is.Null);
        Assert.That(result.Amount, Is.EqualTo(100.0f));
        Assert.That(await _context.Set<Payment>().CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ShouldReturnCorrectPayment()
    {
        // Arrange
        var payment = new Payment
        {
            Id = 1,
            OrderId = 1,
            PaymentTransactionId = "TRANS123",
            PaymentStatus = PaymentStatusEnum.Paid,
            ProcessedAt = DateTime.Now,
            Amount = 100.0f
        };
        await _context.Set<Payment>().AddAsync(payment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByKey(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.OrderId, Is.EqualTo(1));
        Assert.That(result.PaymentTransactionId, Is.EqualTo("TRANS123"));
        Assert.That(result.PaymentStatus, Is.EqualTo(PaymentStatusEnum.Paid));
        Assert.That(result.ProcessedAt, Is.Not.Null);
        Assert.That(result.Amount, Is.EqualTo(100.0f));
    }

    [Test]
    public async Task GetAll_ShouldReturnAllPayments()
    {
        // Arrange
        var payments = new List<Payment>
        {
            new()
            {
                Id = 1, OrderId = 1, PaymentTransactionId = "TRANS1", PaymentStatus = PaymentStatusEnum.Pending,
                Amount = 100.0f
            },
            new()
            {
                Id = 2, OrderId = 2, PaymentTransactionId = "TRANS2", PaymentStatus = PaymentStatusEnum.Paid,
                Amount = 200.0f
            },
            new()
            {
                Id = 3, OrderId = 3, PaymentTransactionId = "TRANS3", PaymentStatus = PaymentStatusEnum.Failed,
                Amount = 150.0f
            }
        };
        await _context.Set<Payment>().AddRangeAsync(payments);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAll();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.Select(p => p.Amount), Is.EquivalentTo(new[] { 100.0f, 200.0f, 150.0f }));
    }

    [Test]
    public async Task Update_ShouldUpdateExistingPayment()
    {
        // Arrange
        var payment = new Payment
        {
            Id = 1,
            OrderId = 1,
            PaymentTransactionId = "TRANS123",
            PaymentStatus = PaymentStatusEnum.Pending,
            ProcessedAt = null,
            Amount = 100.0f
        };
        await _context.Set<Payment>().AddAsync(payment);
        await _context.SaveChangesAsync();

        payment.PaymentStatus = PaymentStatusEnum.Paid;
        payment.ProcessedAt = DateTime.Now;

        // Act
        var result = await _repo.Update(payment);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.PaymentStatus, Is.EqualTo(PaymentStatusEnum.Paid));
        Assert.That(result.ProcessedAt, Is.Not.Null);
        var updatedPayment = await _context.Set<Payment>().FindAsync(1);
        Assert.That(updatedPayment.PaymentStatus, Is.EqualTo(PaymentStatusEnum.Paid));
        Assert.That(updatedPayment.ProcessedAt, Is.Not.Null);
    }

    [Test]
    public async Task Delete_ShouldRemovePaymentFromDatabase()
    {
        // Arrange
        var payment = new Payment
        {
            Id = 1,
            OrderId = 1,
            PaymentTransactionId = "TRANS123",
            PaymentStatus = PaymentStatusEnum.Pending,
            ProcessedAt = null,
            Amount = 100.0f
        };
        await _context.Set<Payment>().AddAsync(payment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.Delete(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(await _context.Set<Payment>().CountAsync(), Is.EqualTo(0));
    }

    [Test]
    public void GetByKey_ShouldThrowExceptionWhenPaymentNotFound()
    {
        // Act & Assert
        Assert.ThrowsAsync<EntityNotFoundException>(async () => await _repo.GetByKey(1));
    }
}