Update v1.0.0
Add new attributes to Employee entity (SSN, EmergencyContact, DateOfBirth).
Add new attributes to LeaveRequest entity (ApprovedById, ApprovalDate, Type).
Optimize Employee <---> LeaveRequest relationship.
Add employee service to serve logic of employee controller.
Add custom validation for ssn to ensure its validation, also make email unique.
Add new class OperationResult response for determining operation result and operation type.
