import http from './http'
import type { Pair, PlayerScheduleRow} from '../types/round'

export async function getRoundPairs(round: number) {
  const response = await http.get<string[]>(`/rounds/${round}`)
  
  // Convert strings like "Alice vs Bob" to Pair objects
  const pairs: Pair[] = response.data.map(line => {
    const [playerA, playerB] = line.split(' vs ')
    return { playerA, playerB }
  })
  
  return pairs
}


// Spelarens schema
export async function getPlayerSchedule(playerIndex: number, numberOfPlayers: number) {
  const playerId = playerIndex 
  const res = await http.get(`/api/RoundRobin/player/${playerId}/schedule/${numberOfPlayers}`)
  
  console.log('Raw backend data:', res.data)

  const playerSchedule: PlayerScheduleRow[] = res.data.map((match: { round: number; player: { id: number; name: string }; opponent: { id: number; name: string } }) => ({
    round: match.round,
    opponent: match.opponent.name
  }))
  
  return playerSchedule
}

// Hämta max antal rundor
export async function getMaxRounds(numberOfPlayers: number = 20) {
  const response = await http.get<number>(`/rounds/max/${numberOfPlayers}`)
  return response.data
}

/* 
// (Om du vill hämta spelarnamn från backend – annars skicka från frontend)
export async function getPlayers(): Promise<Player[]> {
  const res = await http.get<Player[]>(`/api/RoundRobin/players`)
  return res.data
}
 */








