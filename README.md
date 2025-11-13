# Strava Data Consumer Web Application

A comprehensive web application that consumes public data from the Strava API to provide insights into segments, routes, and cycling/running trends.

## Features

- **Segment Explorer**: Discover and explore Strava segments on an interactive map with leaderboards
- **Difficulty Analyzer**: Analyze segments by difficulty level with filtering capabilities
- **Route Recommender**: Find popular routes based on location, distance, and elevation
- **Trends Dashboard**: Track trending segments with time-series analytics
- **Event Finder**: Discover popular segments and potential event locations

## Tech Stack

### Backend
- .NET 8 Core (ASP.NET Core Web API)
- Memory Caching for performance optimization
- Polly for HTTP retry policies
- Swagger/OpenAPI for API documentation

### Frontend
- React 18+ with Vite
- TailwindCSS for styling
- React Query (TanStack Query) for state management
- React Router for navigation
- Leaflet.js for interactive maps
- Recharts for data visualization

## Project Structure

```
stravexplore2/
├── backend/
│   └── StravaConsumer.API/
│       ├── Controllers/          # API endpoints
│       ├── Services/             # Business logic
│       ├── Models/               # DTOs and domain models
│       ├── Program.cs            # Application entry point
│       └── appsettings.json      # Configuration
│
├── frontend/
│   ├── src/
│   │   ├── components/           # React components
│   │   ├── pages/                # Page components
│   │   ├── hooks/                # Custom React hooks
│   │   ├── api/                  # API client
│   │   ├── App.jsx               # Main app component
│   │   └── main.jsx              # Application entry point
│   ├── index.html
│   ├── vite.config.js
│   └── package.json
│
└── README.md
```

## Getting Started

### Prerequisites

- .NET 8 SDK (for backend)
- Node.js 18+ (for frontend)

### Backend Setup

1. Navigate to the backend directory:
```bash
cd backend/StravaConsumer.API
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run
```

The API will be available at `http://localhost:5000`

Swagger documentation: `http://localhost:5000/swagger`

### Frontend Setup

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Start the development server:
```bash
npm run dev
```

The application will be available at `http://localhost:3000`

## API Endpoints

### Segments
- `GET /api/segments/explore` - Explore segments by bounds
- `GET /api/segments/{id}/details` - Get segment details with analysis
- `GET /api/segments/{id}/leaderboard` - Get segment leaderboard
- `GET /api/segments/difficulty-analyzer` - Analyze segments by difficulty
- `POST /api/segments/compare` - Compare multiple segments

### Routes
- `GET /api/routes/popular` - Get popular routes by location
- `GET /api/routes/{id}/details` - Get route details
- `GET /api/routes/{id}/stats` - Get route statistics
- `GET /api/routes/filter` - Filter routes by difficulty

### Trends
- `GET /api/trends/segments` - Get trending segments
- `GET /api/trends/segment/{id}/history` - Get segment trend history
- `GET /api/trends/events` - Find event segments

### Health
- `GET /api/health` - API health check

## Configuration

### Backend (appsettings.json)

```json
{
  "Strava": {
    "BaseUrl": "https://www.strava.com/api/v3",
    "RateLimitPerMinute": 200
  },
  "Caching": {
    "SegmentCacheDurationMinutes": 60,
    "RouteCacheDurationMinutes": 120,
    "TrendsCacheDurationMinutes": 30
  }
}
```

### Frontend (vite.config.js)

The frontend is configured to proxy API requests to `http://localhost:5000`

## Features in Detail

### Segment Explorer
- Interactive map showing segments in the current view
- Click on markers to view segment details
- Real-time leaderboard for selected segments
- Filter by activity type (Ride/Run)

### Difficulty Analyzer
- Segments classified by difficulty (Easy, Moderate, Hard, Very Hard, Extreme)
- Difficulty score based on grade, distance, and elevation
- Filter segments by difficulty level
- Visual map representation

### Route Recommender
- Find popular routes near any location
- Filter by radius
- View route statistics including distance, elevation gain
- Popularity scoring

### Trends Dashboard
- Top 20 trending segments by region
- Time-series charts showing effort and athlete counts
- 30-day trend analysis
- Overall trend percentage calculation

### Event Finder
- Popular segments that could host events
- Estimated participant counts
- PR (Personal Record) statistics
- Activity type filtering

## Caching Strategy

The application implements aggressive caching to respect Strava API rate limits:

- **Segments**: 60 minutes
- **Routes**: 120 minutes
- **Trends**: 30 minutes

## Important Notes

1. **Mock Data**: This application uses mock data for demonstration purposes. In a production environment, you would need:
   - Strava API authentication (OAuth 2.0)
   - Valid API credentials
   - Proper rate limiting implementation

2. **Rate Limiting**: Strava API has limits of 200 requests per 15 minutes and 2000 requests per day

3. **Polyline Encoding**: Segments and routes use Google's polyline encoding format

## Development

### Building for Production

Backend:
```bash
cd backend/StravaConsumer.API
dotnet publish -c Release
```

Frontend:
```bash
cd frontend
npm run build
```

## License

This project is for educational and demonstration purposes.

## Future Enhancements

- [ ] Real Strava API integration with OAuth
- [ ] User authentication and profiles
- [ ] Segment bookmarking and favorites
- [ ] Route planning and GPX export
- [ ] Redis caching for scalability
- [ ] Docker containerization
- [ ] Unit and integration tests
- [ ] Mobile responsive improvements
