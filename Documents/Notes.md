# Feature Development Flow: TDD
- Define Service's Interfaces and its methods
- Write Test cases and make it fail
- Implement the services until all test cases passes
- Complete the flow
# Getting into a course
**Use Case: Enroll in Course**
**Actors:** User, System
**Preconditions:** User is logged in and has access to course enrollment options.
**Basic Flow:**
1. **User selects course** from the available options.
2. **System presents two options:** "Enroll" or "Get it with subscription."
3. **If "Enroll" is selected:**
    - System creates an order for the course.
    - System directs the user to the orders page.
    - User completes payment for the order.
    - System adds the course to the user's course list.
4. **If "Get it with subscription" is selected:**
    - System checks the user's subscription status.
    - If subscription is valid, System adds the course to the user's course list.
**Postconditions:** The user is enrolled in the course and can access course materials.