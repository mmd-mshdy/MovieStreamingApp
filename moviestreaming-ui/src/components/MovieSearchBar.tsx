import {
  useEffect,
  useRef,
  useState,
} from "react";
import { useNavigate } from "react-router-dom";
import {
  LoaderCircle,
  Search,
  X,
} from "lucide-react";
import {
  movieService,
  type MovieDto,
} from "../services/movieService";

const fallbackPoster =
  "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?q=80&w=300";

export const MovieSearchBar = () => {
  const [searchTerm, setSearchTerm] =
    useState("");

  const [results, setResults] =
    useState<MovieDto[]>([]);

  const [loading, setLoading] =
    useState(false);

  const [isOpen, setIsOpen] =
    useState(false);

  const [error, setError] =
    useState<string | null>(null);

  const containerRef =
    useRef<HTMLDivElement>(null);

  const navigate = useNavigate();

  useEffect(() => {
    const normalizedSearchTerm =
      searchTerm.trim();

    if (
      normalizedSearchTerm.length < 2
    ) {
      setResults([]);
      setIsOpen(false);
      setLoading(false);
      setError(null);
      return;
    }

    const controller =
      new AbortController();

    const timeoutId = window.setTimeout(
      async () => {
        try {
          setLoading(true);
          setError(null);

          const movies =
            await movieService.searchMovies(
              normalizedSearchTerm
            );

          if (
            !controller.signal.aborted
          ) {
            setResults(movies);
            setIsOpen(true);
          }
        } catch (searchError) {
          if (
            !controller.signal.aborted
          ) {
            console.error(
              "Movie search failed:",
              searchError
            );

            setResults([]);
            setError(
              "Search is temporarily unavailable."
            );
            setIsOpen(true);
          }
        } finally {
          if (
            !controller.signal.aborted
          ) {
            setLoading(false);
          }
        }
      },
      350
    );

    return () => {
      controller.abort();
      window.clearTimeout(timeoutId);
    };
  }, [searchTerm]);

  useEffect(() => {
    function handleOutsideClick(
      event: MouseEvent
    ) {
      if (
        containerRef.current &&
        !containerRef.current.contains(
          event.target as Node
        )
      ) {
        setIsOpen(false);
      }
    }

    document.addEventListener(
      "mousedown",
      handleOutsideClick
    );

    return () => {
      document.removeEventListener(
        "mousedown",
        handleOutsideClick
      );
    };
  }, []);

  const clearSearch = () => {
    setSearchTerm("");
    setResults([]);
    setIsOpen(false);
    setError(null);
  };

  const openMovie = (
    movieId: string
  ) => {
    clearSearch();

    navigate(`/movies/${movieId}`);
  };

  return (
    <div
      ref={containerRef}
      className="relative w-full max-w-md"
    >
      <div className="relative">
        <Search className="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-500" />

        <input
          type="search"
          value={searchTerm}
          onChange={(event) =>
            setSearchTerm(
              event.target.value
            )
          }
          onFocus={() => {
            if (
              searchTerm.trim().length >=
              2
            ) {
              setIsOpen(true);
            }
          }}
          placeholder="Search movies..."
          aria-label="Search movies"
          autoComplete="off"
          className="h-10 w-full rounded-xl border border-slate-800 bg-slate-900/90 pl-10 pr-10 text-sm text-white outline-none transition placeholder:text-slate-600 focus:border-brandAccent focus:ring-2 focus:ring-brandAccent/20"
        />

        {loading ? (
          <LoaderCircle className="absolute right-3 top-1/2 h-4 w-4 -translate-y-1/2 animate-spin text-brandAccent" />
        ) : (
          searchTerm && (
            <button
              type="button"
              onClick={clearSearch}
              aria-label="Clear search"
              className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-500 transition hover:text-white"
            >
              <X className="h-4 w-4" />
            </button>
          )
        )}
      </div>

      {isOpen && (
        <div className="absolute left-0 right-0 top-12 z-[100] max-h-[420px] overflow-y-auto rounded-2xl border border-slate-800 bg-slate-950/98 p-2 shadow-2xl backdrop-blur-xl">
          {error ? (
            <p className="p-4 text-center text-sm text-rose-400">
              {error}
            </p>
          ) : results.length === 0 &&
            !loading ? (
            <p className="p-4 text-center text-sm text-slate-500">
              No movies found.
            </p>
          ) : (
            results.map((movie) => (
              <button
                key={movie.id}
                type="button"
                onClick={() =>
                  openMovie(movie.id)
                }
                className="flex w-full items-center gap-3 rounded-xl p-2 text-left transition hover:bg-slate-800"
              >
                <img
                  src={
                    movie.posterUrl ||
                    fallbackPoster
                  }
                  alt={movie.title}
                  className="h-16 w-11 shrink-0 rounded-md object-cover"
                  onError={(event) => {
                    event.currentTarget.src =
                      fallbackPoster;
                  }}
                />

                <div className="min-w-0 flex-1">
                  <p className="truncate text-sm font-bold text-white">
                    {movie.title}
                  </p>

                  <div className="mt-1 flex flex-wrap items-center gap-2 text-[10px] text-slate-500">
                    {movie.releaseDate && (
                      <span>
                        {new Date(
                          movie.releaseDate
                        ).getFullYear()}
                      </span>
                    )}

                    {movie.genres?.length >
                      0 && (
                      <>
                        <span>•</span>

                        <span className="truncate">
                          {movie.genres
                            .slice(0, 2)
                            .join(" · ")}
                        </span>
                      </>
                    )}
                  </div>
                </div>
              </button>
            ))
          )}
        </div>
      )}
    </div>
  );
};