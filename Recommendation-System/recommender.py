import pandas as pd
import joblib
import os
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import linear_kernel

class MovieRecommender:
    def __init__(self, model_dir="model_data"):
        self.model_dir = model_dir
        self.movies_df = None
        self.cosine_sim = None
        # Ensure directory exists
        os.makedirs(self.model_dir, exist_ok=True)
        
    def save_model(self):
        """Saves the current model state to disk."""
        if self.movies_df is not None and self.cosine_sim is not None:
            joblib.dump(self.movies_df, os.path.join(self.model_dir, 'movies_df.pkl'))
            joblib.dump(self.cosine_sim, os.path.join(self.model_dir, 'cosine_sim.pkl'))
            print("Model saved successfully.")
        else:
            raise ValueError("No model state to save. Train the model first.")

    def load_model(self):
        """Loads the model state from disk."""
        try:
            self.movies_df = joblib.load(os.path.join(self.model_dir, 'movies_df.pkl'))
            self.cosine_sim = joblib.load(os.path.join(self.model_dir, 'cosine_sim.pkl'))
            print("Model loaded successfully.")
        except FileNotFoundError:
            print("No saved model found. Please train the model.")

    def train(self, movies_list):
        self.movies_df = pd.DataFrame(movies_list)
        self.movies_df['genres'] = self.movies_df['genres'].fillna('')
        
        tfidf = TfidfVectorizer(token_pattern=r'(?u)\b\w+\b')
        tfidf_matrix = tfidf.fit_transform(self.movies_df['genres'])
        
        self.cosine_sim = linear_kernel(tfidf_matrix, tfidf_matrix)
        
        # Automatically save after training
        self.save_model()

    def get_recommendations(self, watched_movie_ids, top_n=5):
        if self.movies_df is None or self.cosine_sim is None:
            return []
            
    def get_status(self):
        """Returns the status and size of the current model."""
        if self.movies_df is None or self.cosine_sim is None:
            return {"trained": False, "movie_count": 0}
        
        return {
            "trained": True,
            "movie_count": len(self.movies_df)
        }
        # Find indices of watched movies that actually exist in our training set
        watched_indices = self.movies_df[self.movies_df['id'].isin(watched_movie_ids)].index.tolist()
        
        if not watched_indices:
            # Fallback: Return top IDs
            return self.movies_df['id'].head(top_n).tolist()

        # Aggregate similarity scores (Average is often better than Sum)
        # We slice the cosine_sim matrix and sum rows for watched movies
        aggregated_scores = self.cosine_sim[watched_indices].sum(axis=0) / len(watched_indices)
        
        # Get indices sorted by score
        recommended_indices = aggregated_scores.argsort()[::-1]
        
        recommended_movie_ids = []
        for idx in recommended_indices:
            movie_id = int(self.movies_df.iloc[idx]['id'])
            if movie_id not in watched_movie_ids:
                recommended_movie_ids.append(movie_id)
            
            if len(recommended_movie_ids) == top_n:
                break
                
        return recommended_movie_ids