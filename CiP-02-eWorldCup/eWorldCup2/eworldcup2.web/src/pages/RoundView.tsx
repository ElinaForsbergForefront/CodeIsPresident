import { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { RoundSelector } from '../components/RoundSelector'
import PairTable from '../components/PairTable'
import InfoPanel from '../components/InfoPanel'
import { getRoundPairs } from '../api/roundRobin'
import type { Pair } from '../types/round'
import { Container, Typography } from '@mui/material'

export default function RoundView() {
  const [pairs, setPairs] = useState<Pair[]>([])
  const { round } = useParams<{ round: string }>()

  const handleRoundSelect = async (roundNumber: number) => {
      const data = await getRoundPairs(roundNumber)
      setPairs(data)
  }

  // Ladda runda automatiskt från URL när komponenten mountas
  useEffect(() => {
    if (round) {
      const roundNumber = parseInt(round, 10)
      if (!isNaN(roundNumber)) {
        handleRoundSelect(roundNumber)
      }
    }
  }, [round])

  return (
    <Container sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Rundvy
      </Typography>

      <InfoPanel />

      <RoundSelector onSelect={handleRoundSelect} />

      {pairs.length > 0 && <PairTable pairs={pairs} />}
    </Container>
  )
}