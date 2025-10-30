import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import RoundView from './pages/RoundView'
import PlayerScheduleView from './pages/PlayerScheduleView'
import Navigation from './components/Navigation'

export default function App() {
  return (
    <BrowserRouter>
      <Navigation />
      <Routes>
        <Route path="/" element={<Navigate to="/player-schedule" replace />} />
        <Route path="/player-schedule" element={<PlayerScheduleView />} />
        <Route path="/round/:round" element={<RoundView />} />
      </Routes>
    </BrowserRouter>
  )
}

