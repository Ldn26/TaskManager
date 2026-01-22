using System.Text.Json.Serialization;



public enum UserRole { Admin, Manager, Member }

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum TaskStatus { Todo, InProgress, Done }
public enum Priority { Low, Medium, High, Critical }
public enum ProjectStatus { Active, OnHold, Completed }