```mermaid
erDiagram
	Educator ||--o{ Course : Creates
	User ||--|| Student : Is
	User ||--|| Educator : Is
    Student ||--o{ Note : Writes
    Student ||--o{ Review : Writes
    Student ||--o{ Question : Asks
    
	Content ||--o| Video : Is
	Content ||--o| Article : Is
	
	Content ||--o{ Question : Receives
	Question ||--o{ Answer : Receives
	
	Course ||--|{ Section : Contains
	Course ||--o{ Review: Receives
	Section ||--|{ Content: Contains
	
	LearningPathCourse ||--|{ Course : Contains
	
	Category ||--o{ Course : Belongs_to
	Skill ||--o{ Course : Belongs_to

	Subscription ||--o{ Course : Provides_access_to	
```


