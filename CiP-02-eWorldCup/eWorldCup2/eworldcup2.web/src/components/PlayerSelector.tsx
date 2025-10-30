import { useEffect, useState } from 'react'
import { FormControl, InputLabel, Select, MenuItem, Stack, Button } from '@mui/material'
import type { Player } from '../types/round'
// import { getPlayers } from '../api/roundRobin' // om du har endpointen

type Props = {
  onSelect: (playerIndex: number) => void
}

// Temporär lista om du inte har /players ännu
const SAMPLE: Player[] = [
  { id: 1, name: 'Alice' }, { id: 2, name: 'Bob' }, { id: 3, name: 'Charlie' },
  { id: 4, name: 'Diana' }, { id: 5, name: 'Ethan' }, { id: 6, name: 'Fiona' },
  { id: 7, name: 'George' }, { id: 8, name: 'Hannah' }, { id: 9, name: 'Isaac' },
  { id: 10, name: 'Julia' }, { id: 11, name: 'Kevin' }, { id: 12, name: 'Laura' },
  { id: 13, name: 'Michael' }, { id: 14, name: 'Nina' }, { id: 15, name: 'Oscar' },
  { id: 16, name: 'Paula' }, { id: 17, name: 'Quentin' }, { id: 18, name: 'Rachel' },
  { id: 19, name: 'Samuel' }, { id: 20, name: 'Tina' }
]

export default function PlayerSelector({ onSelect }: Props) {
  const [players, setPlayers] = useState<Player[]>(SAMPLE)
  const [selected, setSelected] = useState<number>(0) // 0-baserat index

  // Vill du hämta från API istället:
  // useEffect(() => { getPlayers().then(setPlayers) }, [])

  return (
    <Stack direction="row" spacing={2} alignItems="center">
      <FormControl size="small" sx={{ minWidth: 200 }}>
        <InputLabel>Spelare</InputLabel>
        <Select
          label="Spelare"
          value={selected}
          onChange={(e) => setSelected(Number(e.target.value))}
        >
          {players.map((p, idx) => (
            <MenuItem key={p.id} value={idx /* 0-baserat index till backend */}>
              {p.name}
            </MenuItem>
          ))}
        </Select>
      </FormControl>

      <Button variant="contained" onClick={() => onSelect(selected)}>
        Visa schema
      </Button>
    </Stack>
  )
}