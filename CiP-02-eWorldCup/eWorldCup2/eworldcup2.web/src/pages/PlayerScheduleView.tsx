import { useState } from 'react'
import { Container, Typography } from '@mui/material'
import PlayerSelector from '../components/PlayerSelector'
import ScheduleTable from '../components/ScheduleTable'
import InfoPanel from '../components/InfoPanel'
import { getPlayerSchedule } from '../api/roundRobin'
import type { PlayerScheduleRow } from '../types/round'

export default function PlayerScheduleView() {
  const [rows, setRows] = useState<PlayerScheduleRow[]>([])

  const handleSelect = async (playerIndex: number) => {
    try {
      // You need to determine numberOfPlayers here, for example:
      const numberOfPlayers = 20; // Uppdaterat till 20 baserat på backend-koden
      console.log(`Fetching schedule for player ${playerIndex} with ${numberOfPlayers} players`)
      
      const data = await getPlayerSchedule(playerIndex, numberOfPlayers)
      console.log('Received data:', data)
      
      // Säkerställ att data är en array
      if (Array.isArray(data)) {
        setRows(data)
      } else {
        console.error('Data is not an array:', data)
        setRows([]) // Sätt tom array som fallback
      }
    } catch (error) {
      console.error('Error fetching player schedule:', error)
      setRows([]) // Sätt tom array vid fel
    }
  }

  return (
    <Container sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Spelarschema – "Vem möter jag när?"
      </Typography>

      <InfoPanel />

      <PlayerSelector onSelect={handleSelect} />
      {rows.length > 0 && <ScheduleTable rows={rows} />}
    </Container>
  )
}
