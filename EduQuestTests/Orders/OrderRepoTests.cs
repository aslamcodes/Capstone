using EduQuest.Commons;
using EduQuest.Entities;
using EduQuest.Features.Orders;
using Microsoft.EntityFrameworkCore;

namespace EduQuestTests.Orders;

[TestFixture]
public class OrderRepoTests
{
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<EduQuestContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new EduQuestContext(options);
        _repo = new OrderRepo(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private EduQuestContext _context;
    private OrderRepo _repo;

    [Test]
    public async Task Add_ShouldAddOrderToDatabase()
    {
        // Arrange
        var order = new Order
        {
            UserId = 1,
            Price = 100.0f,
            OrderStatus = OrderStatusEnum.Pending,
            CreatedAt = DateTime.Now,
            OrderedCourseId = 1,
            DiscountAmount = 10.0f
        };

        // Act
        var result = await _repo.Add(order);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(1));
        Assert.That(result.Price, Is.EqualTo(100.0f));
        Assert.That(result.OrderStatus, Is.EqualTo(OrderStatusEnum.Pending));
        Assert.That(result.OrderedCourseId, Is.EqualTo(1));
        Assert.That(result.DiscountAmount, Is.EqualTo(10.0f));
        Assert.That(await _context.Orders.CountAsync(), Is.EqualTo(1));
    }

    [Test]
    public async Task GetByKey_ShouldReturnCorrectOrder()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            UserId = 1,
            Price = 100.0f,
            OrderStatus = OrderStatusEnum.Pending,
            CreatedAt = DateTime.Now,
            OrderedCourseId = 1,
            DiscountAmount = 10.0f
        };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetByKey(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.UserId, Is.EqualTo(1));
        Assert.That(result.Price, Is.EqualTo(100.0f));
        Assert.That(result.OrderStatus, Is.EqualTo(OrderStatusEnum.Pending));
        Assert.That(result.OrderedCourseId, Is.EqualTo(1));
        Assert.That(result.DiscountAmount, Is.EqualTo(10.0f));
    }

    [Test]
    public async Task GetAll_ShouldReturnAllOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new()
            {
                Id = 1, UserId = 1, Price = 100.0f, OrderStatus = OrderStatusEnum.Pending, OrderedCourseId = 1
            },
            new()
            {
                Id = 2, UserId = 2, Price = 200.0f, OrderStatus = OrderStatusEnum.Completed, OrderedCourseId = 2
            },
            new()
            {
                Id = 3, UserId = 1, Price = 150.0f, OrderStatus = OrderStatusEnum.Processing, OrderedCourseId = 3
            }
        };
        await _context.Orders.AddRangeAsync(orders);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAll();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.Select(o => o.Price), Is.EquivalentTo(new[] { 100.0f, 200.0f, 150.0f }));
    }

    [Test]
    public async Task Update_ShouldUpdateExistingOrder()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            UserId = 1,
            Price = 100.0f,
            OrderStatus = OrderStatusEnum.Pending,
            CreatedAt = DateTime.Now,
            OrderedCourseId = 1,
            DiscountAmount = 10.0f
        };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        order.OrderStatus = OrderStatusEnum.Completed;
        order.CompletedAt = DateTime.Now;

        // Act
        var result = await _repo.Update(order);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.OrderStatus, Is.EqualTo(OrderStatusEnum.Completed));
        Assert.That(result.CompletedAt, Is.Not.Null);
        var updatedOrder = await _context.Orders.FindAsync(1);
        Assert.That(updatedOrder.OrderStatus, Is.EqualTo(OrderStatusEnum.Completed));
        Assert.That(updatedOrder.CompletedAt, Is.Not.Null);
    }

    [Test]
    public async Task Delete_ShouldRemoveOrderFromDatabase()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            UserId = 1,
            Price = 100.0f,
            OrderStatus = OrderStatusEnum.Pending,
            CreatedAt = DateTime.Now,
            OrderedCourseId = 1,
            DiscountAmount = 10.0f
        };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.Delete(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(await _context.Orders.CountAsync(), Is.EqualTo(0));
    }

    [Test]
    public void GetByKey_ShouldThrowExceptionWhenOrderNotFound()
    {
        // Act & Assert
        Assert.ThrowsAsync<EntityNotFoundException>(async () => await _repo.GetByKey(1));
    }
}