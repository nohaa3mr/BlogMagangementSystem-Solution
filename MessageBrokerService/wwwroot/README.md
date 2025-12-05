# Frontend UI Documentation

This folder contains a complete separated frontend (HTML, CSS, JavaScript) that is coupled with the backend API controllers and CAP event system.

## 📁 Structure

```
wwwroot/
├── css/
│   └── styles.css          # Modern, responsive CSS styling
├── js/
│   ├── api.js              # Centralized API service
│   ├── users.js            # Users page JavaScript
│   ├── blogposts.js        # Blog Posts page JavaScript
│   └── comments.js         # Comments page JavaScript
├── pages/
│   ├── users.html          # Users CRUD page
│   ├── blogposts.html      # Blog Posts CRUD page
│   └── comments.html       # Comments CRUD page
└── index.html              # Dashboard/home page
```

## 🔗 How Everything is Coupled

### 1. Frontend → API Layer

**HTML Pages** → **JavaScript Files** → **API Service** → **Backend Controllers**

```
User Action (HTML)
    ↓
JavaScript Event Handler (users.js)
    ↓
API Service (api.js) - fetch('/api/users', ...)
    ↓
Backend Controller (UsersController.cs)
```

### 2. API Controllers → CAP Events

**Controllers** → **IEventBus** → **CAP Library** → **RabbitMQ** → **Subscribers**

```
UsersController.Create()
    ↓
await _eventBus.Publish(new UserRegisteredEvent())
    ↓
CAPEventBus.Publish()
    ↓
CAP Library → RabbitMQ → SQL Server
    ↓
Subscribers process events asynchronously
```

## 🎯 Complete Flow Example

### Creating a User:

1. **User clicks "Create User" button** (users.html)
2. **JavaScript shows modal** (users.js)
3. **User fills form and submits**
4. **JavaScript calls API** (api.js → `ApiService.createUser()`)
5. **HTTP POST to `/api/users`**
6. **UsersController.Create()** receives request
7. **UserService.CreateAsync()** saves user (in-memory)
8. **UserRegisteredEvent** is published via CAP
9. **Event stored in SQL Server + published to RabbitMQ**
10. **Subscribers process event:**
    - NotificationSubscriber → sends welcome email
    - AnalyticsSubscriber → tracks registration
    - IntegrationSubscriber → syncs to CRM
    - AuditLogSubscriber → logs event
11. **Response returned to frontend**
12. **UI updates** showing success message and new user in table

## 📡 API Endpoints

### Users API (`/api/users`)
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create user → **Publishes UserRegisteredEvent**
- `PUT /api/users/{id}` - Update user → **Publishes UserProfileUpdatedEvent**
- `DELETE /api/users/{id}` - Delete user
- `POST /api/users/{id}/verify-email` → **Publishes UserEmailVerifiedEvent**

### Blog Posts API (`/api/blogposts`)
- `GET /api/blogposts` - Get all posts
- `GET /api/blogposts/{id}` - Get post by ID
- `POST /api/blogposts` - Create post → **Publishes BlogPostCreatedEvent** (and BlogPostPublishedEvent if published)
- `PUT /api/blogposts/{id}` - Update post → **Publishes BlogPostPublishedEvent** if status changes to published
- `DELETE /api/blogposts/{id}` - Delete post
- `POST /api/blogposts/{id}/publish` → **Publishes BlogPostPublishedEvent**

### Comments API (`/api/comments`)
- `GET /api/comments` - Get all comments
- `GET /api/comments?postId={id}` - Get comments by post
- `GET /api/comments/{id}` - Get comment by ID
- `POST /api/comments` - Create comment → **Publishes CommentAddedEvent**
- `PUT /api/comments/{id}` - Update comment
- `DELETE /api/comments/{id}` - Delete comment

## 🎨 UI Features

### Styling
- Modern, clean design with CSS variables
- Responsive layout (mobile-friendly)
- Consistent color scheme and typography
- Smooth animations and transitions

### JavaScript Features
- Modular code structure
- Centralized API service
- Reusable utility functions
- Error handling with user-friendly alerts
- Loading states
- Modal dialogs for create/edit

### Pages

#### Dashboard (`index.html`)
- Overview of the system
- Quick navigation links
- Information about event flow

#### Users Page (`pages/users.html`)
- List all users in a table
- Create new users
- Edit existing users
- Delete users
- Verify email addresses
- Shows email verification status

#### Blog Posts Page (`pages/blogposts.html`)
- List all blog posts
- Create new posts (with author selection)
- Edit posts
- Publish posts
- Delete posts
- Auto-generate slugs from titles
- Shows publication status

#### Comments Page (`pages/comments.html`)
- List all comments
- Create new comments (linked to posts)
- Edit comments
- Delete comments
- Support for threaded comments (parent/child)
- Shows comment author and post

## 🚀 Running the Application

1. **Ensure RabbitMQ is running**:
   ```bash
   docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
   ```

2. **Ensure SQL Server is running** (configured in appsettings.json)

3. **Start the application**:
   ```bash
   dotnet run
   ```

4. **Open browser**:
   ```
   http://localhost:5000 (or https://localhost:5001)
   ```

5. **Navigate through the UI**:
   - Dashboard → Overview
   - Users → Manage users
   - Blog Posts → Manage blog posts
   - Comments → Manage comments

## 📊 Event Flow Visualization

```
┌──────────────┐
│  HTML Form   │
└──────┬───────┘
       │ Submit
       ▼
┌──────────────┐
│  JavaScript  │
│  (users.js)  │
└──────┬───────┘
       │ API Call
       ▼
┌──────────────┐
│  API Service │
│   (api.js)   │
└──────┬───────┘
       │ HTTP Request
       ▼
┌──────────────────┐
│   Controller     │
│(UsersController) │
└──────┬───────────┘
       │ Business Logic
       ▼
┌──────────────┐
│   Service    │
│(UserService) │
└──────┬───────┘
       │ Save Data
       ▼
┌──────────────┐
│  IEventBus   │
│(CAPEventBus) │
└──────┬───────┘
       │ Publish Event
       ▼
┌──────────────┐
│ CAP Library  │
└──────┬───────┘
       │
       ├─→ SQL Server (Storage)
       └─→ RabbitMQ (Message Queue)
              │
              ▼
       ┌──────────────┐
       │  Subscribers │
       │  (Multiple)  │
       └──────────────┘
```

## ✅ Benefits of This Architecture

1. **Separation of Concerns**: Frontend, API, and event handling are separate
2. **Decoupled**: Frontend doesn't know about event subscribers
3. **Scalable**: Easy to add new subscribers without changing frontend
4. **Maintainable**: Clear structure and organization
5. **Testable**: Each layer can be tested independently
6. **Reusable**: API can be used by other clients (mobile, desktop, etc.)

## 🔧 Customization

### Adding New Pages
1. Create HTML file in `wwwroot/pages/`
2. Create JavaScript file in `wwwroot/js/`
3. Link JavaScript in HTML
4. Add navigation link

### Styling
- Modify `wwwroot/css/styles.css`
- CSS variables in `:root` for easy theme changes

### API Integration
- All API calls go through `wwwroot/js/api.js`
- Add new methods to `ApiService` class

## 📝 Notes

- All data is stored in-memory (services use static lists)
- For production, replace with actual database repositories
- Events are persisted in SQL Server via CAP
- Subscribers process events asynchronously
- Frontend doesn't need to wait for subscribers to complete

