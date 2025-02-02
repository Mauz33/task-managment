namespace Tasks.Service.Models;

public class TaskModel
{
    public int Id { get; set; }
    public List<int> AssignToIds { get; set; }
    public int CreatedById { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateDeadline { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TaskStatuses TaskStatusId { get; set; }

    public TaskStatus TaskStatus { get; set; }
}

public class TaskStatus
{
    public TaskStatuses Id { get; set; }
    public string Title { get; set; }
}


public enum TaskStatuses
{
    None = 1,
    InProgress,
    Completed
}