import type { PlayerScheduleRow } from '../types/round'
import { Table, TableHead, TableRow, TableBody, TableCell, Paper, Link, Typography } from '@mui/material'
import { useNavigate } from 'react-router-dom'

type Props = {
  rows: PlayerScheduleRow[]
}

export default function ScheduleTable({ rows }: Props) {
  const navigate = useNavigate()

  const goToRound = (round: number) => {
    // Länka till Rundvy (du kan välja din egen route)
    navigate(`/round/${round}`)
  }

  // Säkerställ att rows är en array
  if (!Array.isArray(rows)) {
    console.error('ScheduleTable: rows is not an array:', rows)
    return (
      <Paper sx={{ mt: 2, p: 2 }}>
        <Typography color="error">
          Fel: Kunde inte ladda schema (data är inte en array)
        </Typography>
      </Paper>
    )
  }

  return (
    <Paper sx={{ mt: 2 }}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Runda</TableCell>
            <TableCell>Motståndare</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {rows.map((r, idx) => (
            <TableRow key={idx} hover>
              <TableCell>
                <Link component="button" onClick={() => goToRound(r.round)}>
                  {r.round}
                </Link>
              </TableCell>
              <TableCell>{r.opponent}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </Paper>
  )
}