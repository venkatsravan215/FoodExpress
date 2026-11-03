import './App.css';
import { useEffect, useState } from 'react';
import { apiRequest } from './services/api';

const categories = [
  { emoji: '🍕', name: 'Pizza' },
  { emoji: '🍔', name: 'Burgers' },
  { emoji: '🍜', name: 'Noodles' },
  { emoji: '🍛', name: 'Biryani' },
  { emoji: '🥗', name: 'Healthy' },
  { emoji: '🍰', name: 'Desserts' },
];

const sampleDishes = [
  { name: 'Chicken Biryani', restaurant: 'FoodExpress Kitchen', price: 250, rating: '4.8', emoji: '🍛', color: '#FFF0D9', time: '25–30 min', category: 'Biryani' },
  { name: 'Classic Burger', restaurant: 'Burger House', price: 180, rating: '4.7', emoji: '🍔', color: '#FBE4D5', time: '20–25 min', category: 'Burgers' },
  { name: 'Margherita Pizza', restaurant: 'Oven Stories', price: 220, rating: '4.6', emoji: '🍕', color: '#F9E6DE', time: '25–35 min', category: 'Pizza' },
  { name: 'Creamy Pasta', restaurant: 'The Italian Table', price: 200, rating: '4.5', emoji: '🍝', color: '#F7EBD7', time: '20–30 min', category: 'Noodles' },
  { name: 'Garden Fresh Salad', restaurant: 'Green Bowl', price: 160, rating: '4.5', emoji: '🥗', color: '#E8F3DF', time: '15–20 min', category: 'Healthy' },
  { name: 'Chocolate Dessert', restaurant: 'Sweet Treats', price: 140, rating: '4.7', emoji: '🍰', color: '#F7E4E5', time: '15–20 min', category: 'Desserts' },
];

function getSavedUser() {
  try {
    const saved = localStorage.getItem('foodexpress_user');
    return saved ? JSON.parse(saved) : null;
  } catch {
    return null;
  }
}

function getDishCategory(item) {
  const text = `${item.name || ''} ${item.description || ''}`.toLowerCase();

  if (text.includes('biryani')) return 'Biryani';
  if (text.includes('pizza')) return 'Pizza';
  if (text.includes('burger')) return 'Burgers';
  if (text.includes('noodle') || text.includes('pasta')) return 'Noodles';
  if (text.includes('salad') || text.includes('healthy') || text.includes('vegan')) return 'Healthy';
  if (text.includes('dessert') || text.includes('cake') || text.includes('chocolate') || text.includes('sweet')) return 'Desserts';

  return 'All';
}

function App() {
  const [favourites, setFavourites] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState('All');
  const [cart, setCart] = useState([]);
  const [backendCartId, setBackendCartId] = useState(null);
  const [showCart, setShowCart] = useState(false);

  const [showLogin, setShowLogin] = useState(false);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loginLoading, setLoginLoading] = useState(false);
  const [loginError, setLoginError] = useState('');
  const [loggedInUser, setLoggedInUser] = useState(getSavedUser);

  const [restaurants, setRestaurants] = useState([]);
  const [menuItems, setMenuItems] = useState([]);
  const [apiStatus, setApiStatus] = useState('');

  const toggleFavourite = (name) => {
    setFavourites((current) =>
      current.includes(name)
        ? current.filter((item) => item !== name)
        : [...current, name]
    );
  };

  const mapCartItems = (items) =>
    (items || []).map((item) => ({
      id: item.id,
      menuItemId: item.menuItemId,
      name: item.menuItemName,
      price: Number(item.price) || 0,
      quantity: Number(item.quantity) || 0,
      emoji: '🍽️',
      color: '#FFF0D9',
    }));

  const refreshBackendCart = async () => {
    const user = getSavedUser();

    if (!user?.userId) {
      setCart([]);
      setBackendCartId(null);
      return;
    }

    const data = await apiRequest(`/api/Carts/user/${user.userId}`);

    if (data?.id) {
      setBackendCartId(data.id);
      setCart(mapCartItems(data.items));
    } else {
      setCart([]);
    }
  };

  const addToCart = async (dish) => {
    if (!localStorage.getItem('foodexpress_token') || !backendCartId) {
      alert('Your cart is not ready. Please log in again.');
      return;
    }

    try {
      const data = await apiRequest(`/api/carts/${backendCartId}/items`, {
        method: 'POST',
        body: JSON.stringify({
          menuItemId: dish.id,
          quantity: 1,
        }),
      });

      if (data && Array.isArray(data.items)) {
        setCart(mapCartItems(data.items));
      } else {
        await refreshBackendCart();
      }
    } catch (error) {
      console.error('Adding item to backend cart failed:', error);
      alert(`Could not add this item. ${error.message}`);
    }
  };

  const changeQuantity = async (name, change) => {
    if (!backendCartId) {
      alert('Your cart is not ready. Please log in again.');
      return;
    }

    const item = cart.find((cartItem) => cartItem.name === name);

    if (!item) return;

    const newQuantity = item.quantity + change;

    try {
      if (newQuantity <= 0) {
        await apiRequest(
          `/api/carts/${backendCartId}/items/${item.id}`,
          { method: 'DELETE' }
        );
      } else {
        await apiRequest(
          `/api/carts/${backendCartId}/items/${item.id}`,
          {
            method: 'PUT',
            body: JSON.stringify(newQuantity),
          }
        );
      }

      await refreshBackendCart();
    } catch (error) {
      console.error('Updating cart quantity failed:', error);
      alert(`Could not update quantity. ${error.message}`);
    }
  };

  const cartCount = cart.reduce(
    (sum, item) => sum + item.quantity,
    0
  );

  const cartTotal = cart.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0
  );

  const loadRestaurants = async () => {
    const data = await apiRequest('/api/Restaurants');
    setRestaurants(Array.isArray(data) ? data : []);
  };

  const loadMenuItems = async () => {
    const data = await apiRequest('/api/MenuItems');

    const emojis = {
      Pizza: '🍕',
      Burgers: '🍔',
      Noodles: '🍜',
      Biryani: '🍛',
      Healthy: '🥗',
      Desserts: '🍰',
      All: '🍽️',
    };

    const colors = {
      Pizza: '#F9E6DE',
      Burgers: '#FBE4D5',
      Noodles: '#F7EBD7',
      Biryani: '#FFF0D9',
      Healthy: '#E8F3DF',
      Desserts: '#F7E4E5',
      All: '#FFF0D9',
    };

    const mappedItems = (Array.isArray(data) ? data : [])
      .filter((item) => item.isAvailable !== false)
      .map((item) => {
        const category = getDishCategory(item);

        return {
          id: item.id,
          name: item.name,
          restaurant: 'FoodExpress Kitchen',
          description: item.description || '',
          price: Number(item.price) || 0,
          rating: '4.8',
          emoji: emojis[category],
          color: colors[category],
          time: '25–30 min',
          category,
          menuCategoryId: item.menuCategoryId,
        };
      });

    setMenuItems(mappedItems);
    return mappedItems.length;
  };

  const loadUserCart = async () => {
    const user = getSavedUser();
    const token = localStorage.getItem('foodexpress_token');

    if (!token || !user?.userId) {
      setBackendCartId(null);
      setCart([]);
      return;
    }

    try {
      const data = await apiRequest(
        `/api/Carts/user/${user.userId}`
      );

      if (data?.id) {
        setBackendCartId(data.id);
        setCart(mapCartItems(data.items));
        console.log('Saved cart restored:', data.id);
      } else {
        setBackendCartId(null);
        setCart([]);
        console.warn('The API returned no cart for this user.');
      }
    } catch (error) {
      console.error(
        'Could not restore the saved cart:',
        error.message
      );

      // Do not create another cart when loading fails.
      // The existing cart may already be saved in the database.
      setApiStatus(
        'Could not restore your cart. Please check the API connection and login.'
      );
    }
  };

  const loadAppData = async () => {
    const results = await Promise.allSettled([
      loadRestaurants(),
      loadMenuItems(),
      loadUserCart(),
    ]);

    const menuResult = results[1];

    if (menuResult.status === 'fulfilled') {
      setApiStatus(
        menuResult.value > 0
          ? `Loaded ${menuResult.value} menu item(s) from the API.`
          : 'No available menu items were returned by the API.'
      );
    } else {
      console.error('Menu loading error:', menuResult.reason);
      setApiStatus('Could not load menu items. Check the API connection.');
    }

    if (results[0].status === 'rejected') {
      console.error('Restaurant loading error:', results[0].reason);
    }
  };

  // Restore API data and the saved cart after a page refresh.
  useEffect(() => {
    const token = localStorage.getItem('foodexpress_token');
    const user = getSavedUser();

    if (token && user?.userId) {
      loadAppData();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleLogin = async (event) => {
    event.preventDefault();
    setLoginLoading(true);
    setLoginError('');

    try {
      const result = await apiRequest('/api/Auth/login', {
        method: 'POST',
        body: JSON.stringify({ email, password }),
      });

      if (!result?.token) {
        throw new Error('The login API did not return a token.');
      }

      const user = {
        userId: result.userId,
        firstName: result.firstName,
        lastName: result.lastName,
        email: result.email,
        role: result.role,
        expiresAt: result.expiresAt,
      };

      localStorage.setItem('foodexpress_token', result.token);
      localStorage.setItem('foodexpress_user', JSON.stringify(user));

      setLoggedInUser(user);
      setShowLogin(false);
      setPassword('');
      setApiStatus('Login successful. Loading restaurants and menu...');

      await loadAppData();
    } catch (error) {
      console.error('Login error:', error);

      setLoginError(
        error.message.includes('401')
          ? 'Invalid email or password.'
          : error.message.includes('Failed to fetch')
            ? 'Cannot reach the API. Check that the backend is running and its HTTPS certificate is trusted.'
            : error.message
      );
    } finally {
      setLoginLoading(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('foodexpress_token');
    localStorage.removeItem('foodexpress_user');

    setLoggedInUser(null);
    setRestaurants([]);
    setMenuItems([]);
    setCart([]);
    setBackendCartId(null);
    setApiStatus('');
  };

  const displayedDishes =
    menuItems.length > 0 ? menuItems : sampleDishes;

  const filteredDishes =
    selectedCategory === 'All'
      ? displayedDishes
      : displayedDishes.filter(
          (dish) => dish.category === selectedCategory
        );

  return (
    <div className="app">
      <header className="navbar">
        <a className="brand" href="#home">
          <span className="brand-icon">🍴</span>
          <span>Food<span className="brand-accent">Express</span></span>
        </a>

        <nav className="nav-links">
          <a className="active" href="#home">Home</a>
          <a href="#categories">Categories</a>
          <a href="#popular">Popular dishes</a>
        </nav>

        <div className="nav-actions">
          {loggedInUser ? (
            <>
              <span
                style={{
                  color: '#263238',
                  fontSize: '14px',
                  fontWeight: 600,
                }}
                title={loggedInUser.email || ''}
              >
                Hi, {loggedInUser.firstName || loggedInUser.email || 'there'}!
              </span>
              <button className="login-button" onClick={handleLogout}>
                Log out
              </button>
            </>
          ) : (
            <button
              className="login-button"
              onClick={() => {
                setLoginError('');
                setShowLogin(true);
              }}
            >
              Log in
            </button>
          )}

          <button className="cart-button" onClick={() => setShowCart(true)}>
            🛒 <span>My Cart ({cartCount})</span>
          </button>
        </div>
      </header>

      <main id="home">
        <section className="hero">
          <div className="hero-content">
            <span className="eyebrow">GOOD FOOD. GOOD MOOD.</span>
            <h1>Your Cravings,<br /><span>Delivered With Love.</span></h1>
            <p>
              Discover delicious meals from your favourite restaurants.
              Fresh flavours, quick delivery and happy moments.
            </p>

            <div className="hero-actions">
              <a className="primary-button" href="#popular">
                Explore menu <span>→</span>
              </a>
              <span className="delivery-note">⚡ Fast delivery to your door</span>
            </div>

            <div className="hero-stats">
              <div><strong>4.8/5</strong><span>Customer rating</span></div>
              <div><strong>30 min</strong><span>Average delivery</span></div>
              <div><strong>Fresh</strong><span>Made with love</span></div>
            </div>
          </div>

          <div className="hero-art">
            <div className="hero-glow"></div>
            <span className="floating-tag tag-top">✨ Made fresh daily</span>
            <div className="hero-food">🍔</div>
            <span className="floating-tag tag-bottom">❤️ Happiness in every bite</span>
            <span className="decor decor-one">✦</span>
            <span className="decor decor-two">✳</span>
          </div>
        </section>

        <section className="category-section" id="categories">
          <div className="section-heading">
            <div>
              <span className="eyebrow">WHAT ARE YOU CRAVING?</span>
              <h2>Explore categories</h2>
            </div>
            <button
              type="button"
              className="text-link"
              onClick={() => {
                setSelectedCategory('All');
                document.getElementById('popular')?.scrollIntoView({ behavior: 'smooth' });
              }}
            >
              Show all →
            </button>
          </div>

          <div className="category-grid">
            {categories.map((category) => (
              <button
                className={`category-card ${selectedCategory === category.name ? 'selected' : ''}`}
                key={category.name}
                onClick={() => setSelectedCategory(category.name)}
                aria-pressed={selectedCategory === category.name}
              >
                <span className="category-emoji">{category.emoji}</span>
                <span>{category.name}</span>
              </button>
            ))}
          </div>
        </section>

        <section className="popular-section" id="popular">
          <div className="section-heading">
            <div>
              <span className="eyebrow">HANDPICKED FOR YOU</span>
              <h2>
                {selectedCategory === 'All' ? 'Popular right now' : selectedCategory}
                {' '}<span>🔥</span>
              </h2>
            </div>
            <button
              type="button"
              className="text-link"
              onClick={() => setSelectedCategory('All')}
            >
              Show all →
            </button>
          </div>

          {loggedInUser && (
            <p
              role="status"
              style={{
                margin: '12px 0 20px',
                padding: '12px 16px',
                borderRadius: '12px',
                background: apiStatus.toLowerCase().includes('could not') ? '#fff0ed' : '#edf7e8',
                color: apiStatus.toLowerCase().includes('could not') ? '#b42318' : '#276738',
                fontSize: '14px',
              }}
            >
              {apiStatus || 'You are logged in.'}
              {apiStatus.toLowerCase().includes('could not') && (
                <button
                  type="button"
                  onClick={loadAppData}
                  style={{
                    marginLeft: '12px',
                    border: 0,
                    background: 'transparent',
                    color: 'inherit',
                    fontWeight: 700,
                    textDecoration: 'underline',
                    cursor: 'pointer',
                  }}
                >
                  Try again
                </button>
              )}
            </p>
          )}

          {loggedInUser && restaurants.length > 0 && (
            <div style={{ marginBottom: '24px' }}>
              <h3>Restaurants from FoodExpress API</h3>
              <div className="category-grid">
                {restaurants.map((restaurant) => (
                  <article
                    className="category-card"
                    key={restaurant.id}
                    style={{ padding: '16px' }}
                  >
                    <span className="category-emoji">🍽️</span>
                    <strong>{restaurant.name}</strong>
                    <span style={{ color: '#777970', fontSize: '12px' }}>
                      {restaurant.address}
                    </span>
                  </article>
                ))}
              </div>
            </div>
          )}

          <div className="dish-grid">
            {filteredDishes.map((dish) => (
              <article className="dish-card" key={dish.id ?? dish.name}>
                <div className="dish-image" style={{ backgroundColor: dish.color }}>
                  <span className="dish-emoji">{dish.emoji}</span>
                  <span className="rating">★ {dish.rating}</span>
                  <button
                    className={`favourite ${favourites.includes(dish.name) ? 'is-favourite' : ''}`}
                    aria-label={`Favourite ${dish.name}`}
                    aria-pressed={favourites.includes(dish.name)}
                    onClick={() => toggleFavourite(dish.name)}
                  >
                    {favourites.includes(dish.name) ? '♥' : '♡'}
                  </button>
                </div>

                <div className="dish-details">
                  <div className="dish-title-row">
                    <h3>{dish.name}</h3>
                    <span className="dish-price">₹{dish.price}</span>
                  </div>
                  <p>{dish.description || dish.restaurant}</p>
                  <div className="dish-footer">
                    <span>◷ {dish.time}</span>

                    <div className="menu-quantity-controls">
                      <button
                        type="button"
                        className="menu-minus-button"
                        aria-label={`Remove one ${dish.name} from cart`}
                        onClick={() => changeQuantity(dish.name, -1)}
                        disabled={!cart.some((item) => item.name === dish.name)}
                      >
                        −
                      </button>

                      <span className="menu-quantity">
                        {cart.find((item) => item.name === dish.name)?.quantity || 0}
                      </span>

                      <button
                        type="button"
                        className="add-button"
                        aria-label={`Add ${dish.name} to cart`}
                        onClick={() => addToCart(dish)}
                      >
                        +
                      </button>
                    </div>
                  </div>
                </div>
              </article>
            ))}
          </div>

          {filteredDishes.length === 0 && (
            <p>No dishes found in this category.</p>
          )}
        </section>

        <section className="promo-banner">
          <div>
            <span className="eyebrow">A LITTLE SOMETHING EXTRA</span>
            <h2>Good food tastes even better together.</h2>
            <p>Find your next favourite meal with FoodExpress.</p>
          </div>
          <span className="promo-emoji">🍱</span>
          <a href="#popular" className="primary-button">Find your meal →</a>
        </section>
      </main>

      <footer className="footer">
        <a className="brand" href="#home">
          <span className="brand-icon">🍴</span>
          <span>Food<span className="brand-accent">Express</span></span>
        </a>
        <p>Made with ❤️ for food lovers.</p>
        <span>© 2026 FoodExpress</span>
      </footer>

      {showLogin && (
        <div
          className="cart-overlay"
          onClick={(event) => {
            if (event.target === event.currentTarget) setShowLogin(false);
          }}
        >
          <section
            className="cart-panel"
            role="dialog"
            aria-modal="true"
            aria-labelledby="login-title"
          >
            <div className="cart-heading">
              <h2 id="login-title">Welcome back!</h2>
              <button
                className="cart-close"
                onClick={() => setShowLogin(false)}
                aria-label="Close login"
              >
                ×
              </button>
            </div>
            <p>Log in to discover your favourite meals.</p>

            <form onSubmit={handleLogin}>
              <label htmlFor="login-email">Email address</label>
              <input
                id="login-email"
                type="email"
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                placeholder="you@example.com"
                autoComplete="email"
                required
                style={{
                  display: 'block',
                  width: '100%',
                  padding: '13px',
                  margin: '8px 0 18px',
                  border: '1px solid #e5dfd5',
                  borderRadius: '10px',
                }}
              />

              <label htmlFor="login-password">Password</label>
              <input
                id="login-password"
                type="password"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder="Enter your password"
                autoComplete="current-password"
                required
                style={{
                  display: 'block',
                  width: '100%',
                  padding: '13px',
                  margin: '8px 0 18px',
                  border: '1px solid #e5dfd5',
                  borderRadius: '10px',
                }}
              />

              {loginError && (
                <p role="alert" style={{ color: '#d93025' }}>
                  {loginError}
                </p>
              )}

              <button
                type="submit"
                className="primary-button"
                disabled={loginLoading}
                style={{ width: '100%' }}
              >
                {loginLoading ? 'Logging in...' : 'Log in →'}
              </button>
            </form>
          </section>
        </div>
      )}

      {showCart && (
        <div
          className="cart-overlay"
          onClick={(event) => {
            if (event.target === event.currentTarget) setShowCart(false);
          }}
        >
          <section
            className="cart-panel"
            role="dialog"
            aria-modal="true"
            aria-labelledby="cart-title"
          >
            <div className="cart-heading">
              <h2 id="cart-title">Your Cart ({cartCount})</h2>
              <button
                className="cart-close"
                onClick={() => setShowCart(false)}
                aria-label="Close cart"
              >
                ×
              </button>
            </div>

            {cart.length === 0 ? (
              <div className="empty-cart">
                <span>🛒</span>
                <h3>Your cart is empty</h3>
                <p>Add delicious dishes and they will appear here.</p>
                <button
                  className="primary-button"
                  onClick={() => {
                    setShowCart(false);
                    document.getElementById('popular')?.scrollIntoView({ behavior: 'smooth' });
                  }}
                >
                  Explore menu →
                </button>
              </div>
            ) : (
              <>
                <div className="cart-items">
                  {cart.map((item) => (
                    <article className="cart-item" key={item.id}>
                      <div
                        className="cart-item-emoji"
                        style={{ backgroundColor: item.color || '#FFF0D9' }}
                      >
                        {item.emoji || '🍽️'}
                      </div>

                      <div className="cart-item-info">
                        <h3>{item.name}</h3>
                        <p>₹{item.price} each</p>
                        <div className="quantity-controls">
                          <button
                            onClick={() => changeQuantity(item.name, -1)}
                            aria-label={`Remove one ${item.name}`}
                          >
                            −
                          </button>
                          <span>{item.quantity}</span>
                          <button
                            onClick={() => changeQuantity(item.name, 1)}
                            aria-label={`Add one ${item.name}`}
                          >
                            +
                          </button>
                        </div>
                      </div>

                      <strong>₹{item.price * item.quantity}</strong>
                    </article>
                  ))}
                </div>

                <div className="cart-total">
                  <span>Total</span>
                  <strong>₹{cartTotal}</strong>
                </div>

                <button
                  className="primary-button checkout-button"
                  onClick={() => {
                    setShowCart(false);

                    if (!loggedInUser) {
                      setLoginError('');
                      setShowLogin(true);
                    } else {
                      alert('Your cart is ready. Online checkout will be connected in the next step.');
                    }
                  }}
                >
                  Proceed to checkout →
                </button>
              </>
            )}
          </section>
        </div>
      )}
    </div>
  );
}

export default App;