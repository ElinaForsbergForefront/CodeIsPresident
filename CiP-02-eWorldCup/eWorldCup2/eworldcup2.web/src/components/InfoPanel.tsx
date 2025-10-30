import { useEffect, useState } from 'react'
import { Card, CardContent, Typography, Grid, Box } from '@mui/material'
import { getMaxRounds } from '../api/roundRobin'

export default function InfoPanel() {
  const [maxRounds, setMaxRounds] = useState<number | null>(null)
  const [error, setError] = useState<string | null>(null)

  // Antal deltagare är hårdkodat till 20 baserat på backend-koden
  const numberOfParticipants = 20

  useEffect(() => {
    const fetchMaxRounds = async () => {
      try {
        const data = await getMaxRounds(numberOfParticipants)
        setMaxRounds(data)
        setError(null)
      } catch (err) {
        console.error('Error fetching max rounds:', err)
        setError('Kunde inte hämta max antal rundor')
        // Fallback: För round robin med n deltagare = n-1 rundor
        setMaxRounds(numberOfParticipants - 1)
      }
    }

    fetchMaxRounds()
  }, [])

  return (
    <Card sx={{ mb: 3 }}>
      <CardContent>
        <Typography variant="h6" gutterBottom>
          Turneringsinformation
        </Typography>
        
        <Grid container spacing={3}>
          <Grid size={6}>
            <Box textAlign="center">
              <Typography variant="h4" color="primary" fontWeight="bold">
                {numberOfParticipants}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Antal deltagare
              </Typography>
            </Box>
          </Grid>
          
          <Grid size={6}>
            <Box textAlign="center">
              <Typography variant="h4" color="primary" fontWeight="bold">
                {maxRounds ?? '?'}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Max antal rundor
                {error && (
                  <Typography variant="caption" display="block" color="error">
                    {error}
                  </Typography>
                )}
              </Typography>
            </Box>
          </Grid>
        </Grid>
      </CardContent>
    </Card>
  )
}