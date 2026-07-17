from pathlib import Path

from pydantic_settings import BaseSettings, SettingsConfigDict


BASE_DIR = Path(__file__).resolve().parent.parent


class Settings(BaseSettings):
    asp_net_api_base_url: str = "https://localhost:7049/api"
    request_timeout_seconds: float = 30.0
    verify_ssl: bool = False

    model_config = SettingsConfigDict(
        env_file=BASE_DIR / ".env",
        env_file_encoding="utf-8",
        case_sensitive=False,
        extra="ignore",
    )

    @property
    def recommendation_catalog_url(self) -> str:
        return (
            f"{self.asp_net_api_base_url.rstrip('/')}"
            "/movies/recommendation-catalog"
        )

    @property
    def movie_catalog_url(self) -> str:
        return self.recommendation_catalog_url


settings = Settings()