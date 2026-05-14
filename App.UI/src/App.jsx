import { useEffect, useState } from 'react'
import { Cloud, CloudLightning, CloudRain, CloudSnow, Sun, Thermometer, Wind } from 'lucide-react'
import './App.css'

function App() {
  const [data, setData] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [city, setCity] = useState('');
  const [suggestions, setSuggestions] = useState([]);
  const [currentLocation, setCurrentLocation] = useState('');
  const [showSuggestions, setShowSuggestions] = useState(false);

  const fetchWeather = (searchCity = '', coords = null) => {
    setLoading(true);
    setError(null);
    setShowSuggestions(false);

    let url = '/weatherforecast';
    if (coords) {
      url += `?lat=${coords.lat}&lon=${coords.lon}&locationName=${encodeURIComponent(coords.name)}`;
    } else if (searchCity) {
      url += `?city=${encodeURIComponent(searchCity)}`;
    }

    fetch(url)
      .then(response => {
        if (!response.ok) {
          throw new Error('Hava durumu verileri alınamadı.');
        }
        return response.json();
      })
      .then(data => {
        setData(data);
        if (data.length > 0) setCurrentLocation(data[0].location);
        setLoading(false);
      })
      .catch(error => {
        setError(error.message);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchWeather();
  }, []);

  useEffect(() => {
    if (city.length < 2) {
      setSuggestions([]);
      return;
    }

    const timer = setTimeout(() => {
      fetch(`/weatherforecast/search?query=${encodeURIComponent(city)}`)
        .then(res => res.json())
        .then(data => {
          setSuggestions(data);
          setShowSuggestions(true);
        })
        .catch(() => setSuggestions([]));
    }, 300);

    return () => clearTimeout(timer);
  }, [city]);

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (!event.target.closest('.search-bar')) {
        setShowSuggestions(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const handleSearch = (e) => {
    e.preventDefault();
    if (city.trim()) {
      setShowSuggestions(false);
      setSuggestions([]);
      fetchWeather(city);
    }
  };

  const handleSelectSuggestion = (suggestion) => {
    const fullLocation = `${suggestion.name}${suggestion.admin1 ? ', ' + suggestion.admin1 : ''}, ${suggestion.country}`;
    setCity(suggestion.name);
    setShowSuggestions(false);
    setSuggestions([]);
    fetchWeather('', {
      lat: suggestion.latitude,
      lon: suggestion.longitude,
      name: fullLocation
    });
  };

  const getWeatherIcon = (summary) => {
    const s = summary.toLowerCase();
    if (s.includes('sun') || s.includes('hot') || s.includes('warm') || s.includes('scorching')) return <Sun size={48} color="#fbbf24" />;
    if (s.includes('rain') || s.includes('drizzle')) return <CloudRain size={48} color="#60a5fa" />;
    if (s.includes('snow') || s.includes('freezing')) return <CloudSnow size={48} color="#93c5fd" />;
    if (s.includes('storm')) return <CloudLightning size={48} color="#fcd34d" />;
    if (s.includes('wind')) return <Wind size={48} color="#94a3b8" />;
    return <Cloud size={48} color="#cbd5e1" />;
  };

  return (
    <div className="container">
      <header className="header">
        <h1>Weather Dashboard</h1>
        <p>Current weather forecasts for {currentLocation || 'your location'}</p>

        <form onSubmit={handleSearch} className="search-bar">
          <div className="search-input-wrapper">
            <input
              type="text"
              placeholder="Search city..."
              value={city}
              onChange={(e) => setCity(e.target.value)}
              onFocus={() => suggestions.length > 0 && setShowSuggestions(true)}
            />
            <button type="submit">Search</button>
          </div>

          {showSuggestions && suggestions.length > 0 && (
            <ul className="suggestions-list">
              {suggestions.map((s, i) => (
                <li key={i} onClick={() => handleSelectSuggestion(s)}>
                  <div className="suggestion-info">
                    <span className="suggestion-name">{s.name}</span>
                    <span className="suggestion-details">
                      {s.admin1 && `${s.admin1}, `}{s.country}
                    </span>
                  </div>
                </li>
              ))}
            </ul>
          )}
        </form>
      </header>

      <main>
        {loading && (
          <div className="loading-container">
            <div className="loader"></div>
          </div>
        )}

        {error && (
          <div className="error-container">
            <p>Error: {error}</p>
          </div>
        )}

        {!loading && !error && (
          <div className="weather-grid">
            {data.map((item, index) => (
              <div key={index} className="weather-card">
                <div style={{ display: 'flex', justifyContent: 'space-between', width: '100%', alignItems: 'center' }}>
                  <span className="date">{new Date(item.date).toLocaleDateString('en-US', { weekday: 'long', month: 'short', day: 'numeric' })}</span>
                  <span style={{ fontSize: '0.8rem', background: 'rgba(255,255,255,0.1)', padding: '2px 8px', borderRadius: '4px', color: 'var(--primary)' }}>{item.location}</span>
                </div>

                <div className="icon-container">
                  {getWeatherIcon(item.summary)}
                </div>

                <div className="temp-container">
                  <span className="temp">{item.temperatureC}</span>
                  <span className="unit">°C</span>
                </div>

                <div className="summary">
                  {item.summary}
                </div>

                <div className="details" style={{ display: 'flex', gap: '1rem', marginTop: '0.5rem', color: 'var(--text-muted)', fontSize: '0.9rem' }}>
                  <span style={{ display: 'flex', alignItems: 'center', gap: '0.25rem' }}>
                    <Thermometer size={14} /> {item.temperatureF}°F
                  </span>
                </div>
              </div>
            ))}
          </div>
        )}
      </main>
    </div>
  )
}

export default App
