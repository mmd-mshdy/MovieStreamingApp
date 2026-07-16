from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):
    movie_catalog_url: str = (
        "https://localhost:7049/api/movies/recommendation-catalog"
    )
    verify_ssl: bool = False

    model_config = SettingsConfigDict(
        env_file=".env",
        env_file_encoding="utf-8",
        case_sensitive=False,
    )


settings = Settings()