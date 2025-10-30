import { useState } from 'react'
import { Button, TextField, Stack } from '@mui/material'
import { useNavigate, useParams } from 'react-router-dom'

type Props = {
  onSelect: (round: number) => void
}

export function RoundSelector({ onSelect }: Props) {
  const { round: urlRound } = useParams<{ round: string }>()
  const navigate = useNavigate()
  const [round, setRound] = useState<number>(urlRound ? parseInt(urlRound, 10) || 1 : 1)

  const handleSelect = () => {
    onSelect(round)
    navigate(`/round/${round}`)
  }

  return (
    <Stack direction="row" spacing={2} alignItems="center">
      <TextField
        label="Runda"
        type="number"
        value={round}
        onChange={(e) => setRound(Number(e.target.value))}
        size="small"
      />
      <Button variant="contained" onClick={handleSelect}>
        Hämta matcher
      </Button>
    </Stack>
  )
}