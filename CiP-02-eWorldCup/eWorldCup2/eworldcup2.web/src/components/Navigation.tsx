import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material'
import { Link, useLocation } from 'react-router-dom'

export default function Navigation() {
  const location = useLocation()

  const isActive = (path: string) => location.pathname === path

  return (
    <AppBar position="static" sx={{ mb: 3 }}>
      <Toolbar>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          eWorld Cup
        </Typography>
        
        <Box sx={{ display: 'flex', gap: 2 }}>
          <Button 
            color="inherit" 
            component={Link} 
            to="/player-schedule"
            variant={isActive('/player-schedule') ? 'outlined' : 'text'}
            sx={{ 
              borderColor: isActive('/player-schedule') ? 'white' : 'transparent',
              color: 'white' 
            }}
          >
            Spelarschema
          </Button>
          
          <Button 
            color="inherit" 
            component={Link} 
            to="/round/1"
            variant={isActive('/round/1') || location.pathname.startsWith('/round/') ? 'outlined' : 'text'}
            sx={{ 
              borderColor: isActive('/round/1') || location.pathname.startsWith('/round/') ? 'white' : 'transparent',
              color: 'white' 
            }}
          >
            Rundvy
          </Button>
        </Box>
      </Toolbar>
    </AppBar>
  )
}