from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from typing import List, Dict
from recommender import MovieRecommender

app = FastAPI(title="Movie Recommendation Engine API")
engine = MovieRecommender()
engine.load_model()

# Pydantic models for request validation
class TrainData(BaseModel):
    movies: List[Dict]  # Expects list of keys: id, title, genres

class RecommendationRequest(BaseModel):
    watched_movie_ids: List[int]
    top_n: int = 5

@app.post("/train")
def train_model(data: TrainData):
    try:
        engine.train(data.movies)
        return {"status": "success", "message": f"Successfully trained on {len(data.movies)} movies."}
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

@app.post("/recommend")
def get_recommendations(request: RecommendationRequest):
    try:
        recommendations = engine.get_recommendations(request.watched_movie_ids, request.top_n)
        return {"recommended_movie_ids": recommendations}
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
@app.get("/status")
def get_model_status():
    """Checks if the recommendation engine is trained and ready."""
    return engine.get_status()