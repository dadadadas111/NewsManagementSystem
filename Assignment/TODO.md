# News Management System - TODO

## 1. Details View
DONE

## 2. Create Functionality
### API
- Implement POST endpoints for creating NewsArticle, Category, Tag, and SystemAccount if not already present.
- Apply data type validation and model validation attributes (e.g., [Required], [StringLength], [Range]) for all DTO fields.
- Return appropriate error responses for invalid data.

### WebUI
- Add a user-friendly form for creating each entity (NewsArticle, Category, Tag, SystemAccount).
- Use proper input types, validation messages, and dropdowns for related entities.
- Ensure the UI is modern, accessible, and responsive.

## 3. Update Functionality
### API
- Implement PUT endpoints for updating entities if not already present.
- Apply the same validation as in create functionality.

### WebUI
- Add edit forms for each entity, pre-populated with existing data.
- Use the same UI/UX standards as the create forms.

## 4. Authentication and Authorization
### API
- Add endpoints for user registration and login (e.g., /api/auth/register, /api/auth/login).
- Implement JWT-based authentication.
- Add role-based authorization to API endpoints (e.g., only Admin can create/update/delete, regular users can only view).
- Extract user role from JWT claims and enforce access control.

### WebUI
- Add a login screen that is required before accessing any other page.
- Store authentication token securely (e.g., in HttpOnly cookies or local storage).
- Show/hide UI elements based on user role (e.g., hide admin features for regular users).
- Implement logout functionality.

## 5. Additional Tasks
- Add error handling and user-friendly error messages in both API and WebUI.
- Add loading indicators and feedback for async operations in the WebUI.
- Write unit and integration tests for API endpoints and WebUI forms.
- Improve API and UI documentation (Swagger for API, help text/tooltips for UI).
- Refactor and clean up code for maintainability (e.g., use AutoMapper for DTO mapping, organize services/repositories).
- Ensure accessibility (a11y) and responsive design for all WebUI pages.
- Add support for soft delete (mark as inactive) for entities if required.
- Implement audit logging for create/update/delete actions.

