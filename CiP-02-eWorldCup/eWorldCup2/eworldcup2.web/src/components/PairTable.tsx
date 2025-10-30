import type { Pair } from '../types/round'
import { Table, TableHead, TableRow, TableBody, TableCell, Paper } from '@mui/material'

type Props = {
  pairs: Pair[]
}

export default function PairTable({ pairs }: Props) {
  return (
    <Paper sx={{ mt: 2 }}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Player 1</TableCell>
            <TableCell>Player 2</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {pairs.map((p, i) => (
            <TableRow key={i}>
              <TableCell>{p.playerA}</TableCell>
              <TableCell>{p.playerB}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </Paper>
  )
}
