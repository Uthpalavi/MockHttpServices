using Microsoft.AspNetCore.Mvc;
using Moq;
using MockHttpServices.Controllers;
using MockHttpServices.Services;
using MockHttpServices.Models;

namespace TestMockHttpServices;

public class TaskControllerTests
{
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly TaskController _taskController;

    public TaskControllerTests()
    {
        _mockTaskService = new Mock<ITaskService>();
        _taskController = new TaskController(_mockTaskService.Object);
    }

    [Fact(Skip = "just")]
    public void GetTasks_ReturnsOkResult_WithListOfTasks()
    {
        // Arrange
        var taskList = new List<TaskModel> { new TaskModel { Id = 1, Duration = 10, IsCompleted = false } };
        _mockTaskService.Setup(service => service.GetTasks()).Returns(taskList);

        // Act
        var result = _taskController.GetTasks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<TaskModel>>(okResult.Value);
        Assert.Equal(taskList.Count, returnValue.Count);
    }
    

    [Fact(Skip = "just")]
    public async Task StartProcessing_ReturnsOk()
    {
        // Arrange
        _mockTaskService.Setup(service => service.ProcessTasks()).Returns(Task.CompletedTask);

        // Act
        var result = await _taskController.StartProcessing();

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        _mockTaskService.Verify(service => service.ProcessTasks(), Times.Once);
    }

    [Fact(Skip = "just")]
    public void StopProcessing_ReturnsOk()
    {
        // Act
        var result = _taskController.StopProcessing();

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        _mockTaskService.Verify(service => service.StopProcessing(), Times.Once);
    }

    [Fact]
    public void SubmitTasks_ValidDurations_ReturnsAccepted()
    {
        // Arrange
        var validDurations = new List<int> { 10, 20, 25 };

        // Act
        var result = _taskController.SubmitTasks(validDurations);

        // Assert
        var acceptedResult = Assert.IsType<AcceptedResult>(result);
        _mockTaskService.Verify(service => service.AddTasks(validDurations), Times.Once);
    }

    [Fact]
    public void SubmitTasks_InvalidDurations_ReturnsBadRequest()
    {
        // Arrange
        var validDurations = new List<int> { 10, 30, 25 };

        // Act
        var result = _taskController.SubmitTasks(validDurations);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.Equal("Invalid task durations. All durations must be between 10 and 25 seconds.", badRequestResult.Value);
    }

    [Fact]
    public void SubmitTasks_ReturnsBadRequest_WhenTaskDurationsIsNull()
    {
        // Arrange
        List<int> taskDurations = null;

        // Act
        var result = _taskController.SubmitTasks(taskDurations);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Missing 'task_durations_in_seconds' array.", badRequestResult.Value);
    }

    [Fact]
    public void SubmitTasks_ReturnsBadRequest_WhenTaskDurationsIsEmpty()
    {
        // Arrange
        var taskDurations = new List<int>();

        // Act
        var result = _taskController.SubmitTasks(taskDurations);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.Equal("Task durations array is empty.", badRequestResult.Value);
    }

    [Fact(Skip ="just")]
    public void SubmitTasks_ReturnsBadRequest_WhenTaskDurationsContainNonIntegerValues()
    {
        // Arrange
        var taskDurations = new List<object> { 10, 16, 13, 19, 25, "21" };

        // Act
        var result = _taskController.SubmitTasks(taskDurations.Cast<int>().ToList());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.Equal("Some Values in the array are not int.", badRequestResult.Value);
    }
}
