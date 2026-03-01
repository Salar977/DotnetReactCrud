import Navbar from "./components/NavBar"
import Home from "./pages/Home"
import About from "./pages/About"
import NotFound from "./pages/NotFound"
import User from "./components/user/User"
import { Routes, Route } from "react-router-dom";
import { Toaster } from "react-hot-toast"

const App = () => {
  return (
    <>
      <Navbar />

      <Routes>
        <Route index element={<Home />} />
        <Route path="about" element={<About />} />
        <Route path="user" element={<User />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
      <Toaster />
    </>
  )
}

export default App