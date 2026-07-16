from pydantic import BaseModel, Field


class MovieCatalogItem(BaseModel):
    movieId: str
    title: str
    description: str = ""
    genres: list[str] = Field(default_factory=list)
    castMembers: list[str] = Field(default_factory=list)
    releaseYear: int
    averageRating: float = 0.0


class RecommendationInteraction(BaseModel):
    movieId: str
    watchPercentage: float = Field(default=0, ge=0, le=100)
    completed: bool = False
    rating: int | None = Field(default=None, ge=1, le=5)
    inWatchlist: bool = False


class RecommendationRequest(BaseModel):
    userId: str
    interactions: list[RecommendationInteraction] = Field(
        default_factory=list
    )
    topN: int = Field(default=10, ge=1, le=50)


class RecommendationResult(BaseModel):
    movieId: str
    score: float
    reason: str


class RecommendationResponse(BaseModel):
    recommendations: list[RecommendationResult]


class SimilarMovieResponse(BaseModel):
    sourceMovieId: str
    recommendations: list[RecommendationResult]


class EngineStatus(BaseModel):
    trained: bool
    movieCount: int